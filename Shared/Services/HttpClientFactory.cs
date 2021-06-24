using System;
using System.Net.Http;

namespace Shared
{
    public interface IHttpClientFactory : IDisposable
    {
        HttpClient Create();
    }

    public class HttpClientFactory : IHttpClientFactory
    {
        private readonly HttpClient _httpClient;

        public HttpClientFactory()
        {
            _httpClient = new HttpClient();

            _httpClient.DefaultRequestHeaders.Add(@"User-Agent",
                @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.101 Safari/537.36");
        }

        public HttpClient Create()
        {
            return _httpClient;
        }

        public void Dispose()
        {
            _httpClient.Dispose();
        }
    }
}