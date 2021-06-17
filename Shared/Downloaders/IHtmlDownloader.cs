using System.Threading.Tasks;

namespace Shared
{
    public interface IHtmlDownloader
    {
        Task<string> Download(string url);
    }

}