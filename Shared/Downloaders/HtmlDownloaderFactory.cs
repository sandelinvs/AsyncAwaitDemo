using System.Net.Http;

namespace Shared
{

    // redundant?
    public class HtmlDownloaderFactory : IHtmlDownloaderFactory
    {
        private readonly HttpClient _httpClient;

        public HtmlDownloaderFactory(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public IHtmlDownloader Create()
        {
            return new HtmlDownloader(_httpClient);
        }
    }
}