using System;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Services
{
    public interface IDownloader : IDisposable, IAsyncDisposable
    {
        string LocalPath { get; }
        string Url { get; }

        event DownloadHandler OnComplete;
        event DownloadHandler OnStart;

        Task DownloadAsync(CancellationToken cancellationToken = default);
    }
}