using System;
using System.Threading.Tasks;

namespace Shared
{
    public interface IHTMLDownloader
    {
        Task<string> Download(string url);
    }

    public class HTMLDownloader : IHTMLDownloader
    {
        public async Task<string> Download(string url)
        {
            throw new NotImplementedException();
        }
    }
}
