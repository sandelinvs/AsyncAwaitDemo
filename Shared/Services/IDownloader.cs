using System;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Services
{
    public interface IDownloader : IDisposable, IAsyncDisposable
    {
        string LocalPath { get; }
        string Url { get; }

        Task DownloadAsync(CancellationToken cancellationToken = default);
    }
}