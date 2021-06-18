using System.Net.Http;

namespace Shared
{

    public class HtmlDownloaderFactory : IHtmlDownloaderFactory
    {
        private readonly HttpClient _httpClient = new HttpClient();

        public IHtmlDownloader Create()
        {
            return new HtmlDownloader(_httpClient);
        }
    }
}