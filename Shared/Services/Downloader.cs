using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Shared.Sources;

namespace Shared.Services
{
    public delegate Task DownloadHandler(Downloader sender, DownloadEventArgs e);

    public class Downloader : IDownloader
    {
        private readonly IStreamCopy _copier;

        

        public event DownloadHandler OnComplete;
        public event DownloadHandler OnStart;

        public string Url { get; }

        public string LocalPath { get; }

        public Downloader(IStreamCopy copier, string url, string localPath)
        {
            _copier = copier;
            Url = url;
            LocalPath = localPath;
        }

        public async Task DownloadAsync(CancellationToken cancellationToken = default)
        {
            if (OnStart is not null)
                await OnStart(this, new DownloadEventArgs { Url = Url, LocalPath = Url });

            await _copier.Copy(cancellationToken);

            if (OnComplete is not null)
                await OnComplete(this, new DownloadEventArgs { Url = Url, LocalPath = Url });
        }

        public void Dispose()
        {
            _copier.Dispose();
        }

        public async ValueTask DisposeAsync()
        {
            await _copier.DisposeAsync();
        }

        public static async Task DownloadAll(IEnumerable<Downloader> downloaders, CancellationToken cancellationToken)
        {
            await Task.Run(() => Parallel.ForEach(downloaders,
                new ParallelOptions
                {
                    MaxDegreeOfParallelism = 3,
                    CancellationToken = cancellationToken
                },
                async (dl, state) =>
                {
                    try
                    {
                        await dl.DownloadAsync(cancellationToken);
                    }
                    catch
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                    }
                }));
        }
    }
}
