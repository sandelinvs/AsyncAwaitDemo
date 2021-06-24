using System.Threading.Tasks;

namespace Shared.Services
{
    public interface IDownloaderFactory
    {
        Task<IDownloader> Create(string url, string localPath);
    }
}