using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Sources
{
    public sealed class HttpFileSource : IAsyncFileSource, IDisposable, IAsyncDisposable
    {
        private readonly HttpClient _httpClient;
        private readonly bool _needsDispose;
        private readonly string _url;

        public HttpFileSource(string url, HttpClient httpClient = default)
        {
            if (httpClient == default)
            {
                _needsDispose = true;
                _httpClient = new HttpClient();
            }
            else 
            {
                _httpClient = httpClient;
            }

            _url = url;
        }

        public async Task<Stream> GetStreamAsync(CancellationToken cancellationToken)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(_url, cancellationToken);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStreamAsync();
        }

        public void Dispose()
        {
            if (_needsDispose)
                _httpClient.Dispose();
        }

        public async ValueTask DisposeAsync()
        {
            if (_needsDispose)
                _httpClient.Dispose();

            await Task.CompletedTask;
        }
    }
}
