﻿using System.Net.Http;

namespace Shared
{
    public interface IHtmlDownloaderFactory
    {
        IHtmlDownloader Create();
    }

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