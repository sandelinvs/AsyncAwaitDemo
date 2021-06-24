using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Shared;
using Shared.Services;
using Shared.Sources;

namespace Tests
{
    [TestFixture]
    public class Tests
    {
        const string BOOKSTORE_URL = @"https://www.sfbok.se/katalog/bocker-tidningar/romaner-noveller";

        private static HttpClient Client = new HttpClient();

        [Test]
        public async Task Can_download_and_parse_some_html()
        {
            byte[] buffer = new byte[1024 * 10 * 10 * 10];

            await using var source = new HttpFileSource(BOOKSTORE_URL, Client);

            await using var destination = new MemorySource(buffer);

            await new StreamCopy(source, destination).Copy();

            string result = Encoding.Default.GetString(buffer);

            IBookListParser parser = new SFBokBookListParser();

            IEnumerable<BookTitle> titles = parser.Parse(result);

            Assert.IsNotEmpty(titles);
            Assert.IsFalse(String.IsNullOrWhiteSpace(titles.First().Author));
            Assert.IsFalse(String.IsNullOrWhiteSpace(titles.First().AuthorUrl));
            Assert.IsFalse(String.IsNullOrWhiteSpace(titles.First().Title));
            Assert.IsFalse(String.IsNullOrWhiteSpace(titles.First().TitleUrl));
            Assert.IsFalse(String.IsNullOrWhiteSpace(titles.First().CoverArtUrl));
        }

    }

    [TestFixture]
    public class DownloaderSetupFixture
    {
        protected IHttpClientFactory HttpClientFactory;
        protected DownloaderFactory DownloaderFactory;

        public virtual IAsyncFileSource HttpSourceFileFactory(string url)
        {
            return new HttpFileSource(url, HttpClientFactory.Create());
        }

        public virtual IAsyncFileSource FileSystemSourceFactory(string localPath)
        {
            return new FileSystemSource(localPath);
        }

        public virtual IStreamCopy StreamCopyFactory(IAsyncFileSource source, IAsyncFileSource destination)
        {
            return new StreamCopy(source, destination);
        }

        [SetUp]
        public virtual void Setup()
        {
            HttpClientFactory = new HttpClientFactory();
            DownloaderFactory = new DownloaderFactory(HttpSourceFileFactory, FileSystemSourceFactory, StreamCopyFactory);
        }

        [TearDown]
        public virtual void TearDown()
        {
            HttpClientFactory.Dispose();
            HttpClientFactory = null;
            DownloaderFactory = null;
        }
    }

    [TestFixture]
    public class DownloaderTests : DownloaderSetupFixture
    {
        public const string BookStoreUrl = @"https://www.sfbok.se/katalog/bocker-tidningar/romaner-noveller";
        public const string BookStoreImageUrl = @"https://www.sfbok.se/sites/default/files/styles/teaser/sfbok/sfbokbilder/403/403426.jpg?bust=1624032882&itok=WJ8Bb2y1";

        public string LocalFolder => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        public static async Task OnStart(object sender, DownloadEventArgs args)
        {
            TestContext.WriteLine($"Download started: {args.Url}");
            await Task.CompletedTask;
        }

        public static async Task OnComplete(object sender, DownloadEventArgs args)
        {
            TestContext.WriteLine($"Download Completed: {args.Url}");
            await Task.CompletedTask;
        }

        [Test]
        public async Task Can_Download_booklist()
        {
            using var downloader = await DownloaderFactory.Create(BookStoreUrl, Path.Combine(LocalFolder, "booklist.html"));

            downloader.OnStart += OnStart;
            downloader.OnComplete += OnComplete;

            await downloader.DownloadAsync();
        }

        [Test]
        public async Task Can_Download_a_book_cover()
        {
            using var downloader = await DownloaderFactory.Create(BookStoreImageUrl, Path.Combine(LocalFolder, "covertart.jpg"));

            downloader.OnStart += OnStart;
            downloader.OnComplete += OnComplete;

            await downloader.DownloadAsync();
        }
    }
}