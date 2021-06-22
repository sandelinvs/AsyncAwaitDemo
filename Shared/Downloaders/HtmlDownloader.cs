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
        }

        public async Task<string> Download(string url)
        {
            HttpResponseMessage response = await _httpClient.GetAsync(url);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}