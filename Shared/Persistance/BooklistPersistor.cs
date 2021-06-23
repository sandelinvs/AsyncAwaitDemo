using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Shared.Sources;

namespace Shared.Persistance
{
    public class BooklistPersistor
    {
        public List<BookTitle> Books { get; } = new List<BookTitle>();

        public BooklistPersistor()
        {
            
        }

        public async Task Save()
        {
            
        }

        private async Task DownloadCovertArt()
        {
            
        }
    }

    public class DownloadEventArgs : EventArgs 
    {
        public string Url { get; set; }

        public string LocalPath { get; set; }
    }

    public class Downloader : IDisposable, IAsyncDisposable
    {
        private readonly IStreamCopy _copier;

        public delegate Task DownloadHandler(Downloader sender, DownloadEventArgs e);

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

        public async Task Download(CancellationToken cancellationToken)
        {
            if(OnStart is not null)
                await OnStart(this, new DownloadEventArgs { Url = Url, LocalPath = Url });

            await _copier.Copy(cancellationToken);

            if(OnComplete is not null)
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
                        await dl.Download(cancellationToken);
                    }
                    catch 
                    {
                        cancellationToken.ThrowIfCancellationRequested();
                    }
                }));
        }
    }

    public class DownloaderFactory
    {
        private readonly Func<string, IAsyncFileSource> _httpFileSourceFactory;
        private readonly Func<string, IAsyncFileSource> _fileSystemSourceFactory;
        private readonly Func<IAsyncFileSource, IAsyncFileSource, IStreamCopy> _streamCopyFactory;

        public DownloaderFactory(
            Func<string, IAsyncFileSource> httpFileSourceFactory,
            Func<string, IAsyncFileSource> fileSystemSourceFactory,
            Func<IAsyncFileSource, IAsyncFileSource, IStreamCopy> streamCopyFactory)
        {
            _httpFileSourceFactory = httpFileSourceFactory;
            _fileSystemSourceFactory = fileSystemSourceFactory;
            _streamCopyFactory = streamCopyFactory;
        }

        public async Task<Downloader> Create(string url, string localPath) 
        {
            IAsyncFileSource source = _httpFileSourceFactory(url);

            IAsyncFileSource destination = _fileSystemSourceFactory(localPath);

            IStreamCopy copier = _streamCopyFactory(source, destination);

            Downloader downloader = new Downloader(copier, url, localPath);

            return await Task.FromResult(downloader);
        }
    }
}
