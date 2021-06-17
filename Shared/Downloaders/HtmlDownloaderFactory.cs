using System.Net.Http;

namespace Shared
{

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