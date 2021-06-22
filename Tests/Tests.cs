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
using Shared.Sources;

namespace Tests
{
    [TestFixture]
    public class Tests
    {
        const string BOOKSTORE_URL = @"https://www.sfbok.se/katalog/bocker-tidningar/romaner-noveller";
        const string BOOKSTORE_IMAGE_URL = @"https://www.sfbok.se/sites/default/files/styles/teaser/sfbok/sfbokbilder/403/403426.jpg?bust=1624032882&itok=WJ8Bb2y1";

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

        [Test]
        public async Task Can_download_image_and_save_to_disk()
        {
            var folder = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var file = Path.Combine(folder, "downloaded_image.jpg");

            await using var source = new HttpFileSource(BOOKSTORE_IMAGE_URL, Client);

            await using var destination = new FileSystemSource(file);

            await new StreamCopy(source, destination).Copy();
        }
    }
}