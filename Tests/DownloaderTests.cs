using System.IO;
using System.Threading.Tasks;
using NUnit.Framework;
using Shared.Services;

namespace Tests
{
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