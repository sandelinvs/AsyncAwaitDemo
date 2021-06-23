using System;
using System.Threading.Tasks;
using Shared.Sources;

namespace Shared.Services
{
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
