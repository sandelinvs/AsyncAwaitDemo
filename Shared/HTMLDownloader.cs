using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Shared
{
    public interface IHtmlDownloader
    {
        Task<string> Download(string url);
    }

    public class HtmlDownloader : IHtmlDownloader
    {
        private readonly HttpClient _httpClient;

        public HtmlDownloader(HttpClient httpClient)
        {
            _httpClient = httpClient;

            httpClient.DefaultRequestHeaders.Add(@"User-Agent",
                @"Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/91.0.4472.101 Safari/537.36");
        }

        public async Task<string> Download(string url)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }

}