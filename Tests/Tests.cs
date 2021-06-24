using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Shared;
using Shared.Services;
using Shared.Sources;

namespace Tests
{
    //[TestFixture]
    //public class Tests
    //{
    //    const string BOOKSTORE_URL = @"https://www.sfbok.se/katalog/bocker-tidningar/romaner-noveller";

    //    private static HttpClient Client = new HttpClient();

    //    [Test]
    //    public async Task Can_download_and_parse_some_html()
    //    {
    //        byte[] buffer = new byte[1024 * 10 * 10 * 10];

    //        await using var source = new HttpFileSource(BOOKSTORE_URL, Client);

    //        await using var destination = new MemorySource(buffer);

    //        await new StreamCopy(source, destination).Copy();

    //        string result = Encoding.Default.GetString(buffer);

    //        IBookListParser parser = new SFBokBookListParser();

    //        IEnumerable<BookTitle> titles = parser.Parse(result);

    //        Assert.IsNotEmpty(titles);
    //        Assert.IsFalse(String.IsNullOrWhiteSpace(titles.First().Author));
    //        Assert.IsFalse(String.IsNullOrWhiteSpace(titles.First().AuthorUrl));
    //        Assert.IsFalse(String.IsNullOrWhiteSpace(titles.First().Title));
    //        Assert.IsFalse(String.IsNullOrWhiteSpace(titles.First().TitleUrl));
    //        Assert.IsFalse(String.IsNullOrWhiteSpace(titles.First().CoverArtUrl));
    //    }

    //}

    [TestFixture]
    public class DownloaderSetupFixture
    {
        protected IHttpClientFactory HttpClientFactory;
        protected DownloaderFactory DownloaderFactory;

        protected string LocalFolder => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "Downloads");

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

        [OneTimeSetUp]
        public virtual void OneTimeSetup()
        {
            if (!Directory.Exists(LocalFolder))
                Directory.CreateDirectory(LocalFolder);
        }

        [SetUp]
        public virtual void Setup()
        {
            HttpClientFactory = new HttpClientFactory();
            DownloaderFactory = new DownloaderFactory(HttpSourceFileFactory, FileSystemSourceFactory, StreamCopyFactory);

            Downloader.OnStart += OnStart;
            Downloader.OnComplete += OnComplete;
        }

        [TearDown]
        public virtual void TearDown()
        {
            HttpClientFactory.Dispose();
            HttpClientFactory = null;
            DownloaderFactory = null;

            Downloader.OnStart -= OnStart;
            Downloader.OnComplete -= OnComplete;
        }

        public static async void OnStart(object sender, DownloadEventArgs args)
        {
            TestContext.WriteLine($"Download started: {args.Url}");
        }

        public static async void OnComplete(object sender, DownloadEventArgs args)
        {
            TestContext.WriteLine($"Download Completed: {args.Url}");
        }
    }

    [TestFixture]
    public class DownloaderTests : DownloaderSetupFixture
    {
        [Test]
        public async Task Can_Download_booklistpage_to_disk()
        {
            var bookStoreUrl = @"https://www.sfbok.se/katalog/bocker-tidningar/romaner-noveller"; ;

            using var downloader = await DownloaderFactory.Create(bookStoreUrl, Path.Combine(LocalFolder, "booklist.html"));

            await downloader.DownloadAsync();
        }

        [Test]
        public async Task Can_Download_a_bookcover_to_disk()
        {
            var bookStoreImageUrl = @"https://www.sfbok.se/sites/default/files/styles/teaser/sfbok/sfbokbilder/403/403426.jpg?bust=1624032882&itok=WJ8Bb2y1";

            using var downloader = await DownloaderFactory.Create(bookStoreImageUrl, Path.Combine(LocalFolder, "covertart.jpg"));

            await downloader.DownloadAsync();
        }
    }
}