using System;
using System.Net.Http;
using System.Threading.Tasks;
using NUnit.Framework;
using Shared;

namespace Tests
{
    [TestFixture]
    public class Tests
    {
        const string BOOKSTORE_URL = @"https://www.sfbok.se/katalog/bocker-tidningar/romaner-noveller";

        private static HttpClient Client = new HttpClient();

        [Test]
        public async Task Can_download_some_html()
        {
            IHtmlDownloader downloader = new HtmlDownloader(Client);

            string result = await downloader.Download(BOOKSTORE_URL);

            Console.WriteLine(result);

            Assert.Pass();
        }


        [Test]
        public async Task Can_parse_some_html_from_SFBokhandeln()
        {
            IHtmlDownloader downloader = new HtmlDownloader(Client);

            string result = await downloader.Download(BOOKSTORE_URL);

            Console.WriteLine(result);

            IBookListParser parser = new SFBokBookListParser();

            var titles = parser.Parse(result);

            Assert.Pass();
        }

        [Test]
        public void Test()
        {
            var uri = new Uri(new Uri("http://www.google.com/absol/dfds"), "/file/something");

            Console.WriteLine(uri.AbsoluteUri);

            var i = 5;

            Console.WriteLine(i += 5);
        }
    }
}