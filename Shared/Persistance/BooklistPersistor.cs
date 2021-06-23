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

    public class CovertArtDownloader
    {
        private readonly IStreamCopy _copier;

        public CovertArtDownloader(IStreamCopy copier)
        {
            _copier = copier;
        }

        public async Task Download(CancellationToken cancellationToken)
        {
            await _copier.Copy(cancellationToken);
        }
    }

    public class CovertArtDownloaderFactory
    {
        private readonly Func<string, IAsyncFileSource> _httpFileSourceFactory;
        private readonly Func<string, IAsyncFileSource> _fileSystemSourceFactory;
        private readonly Func<IAsyncFileSource, IAsyncFileSource, IStreamCopy> _streamCopyFactory;

        public CovertArtDownloaderFactory(
            Func<string, IAsyncFileSource> httpFileSourceFactory,
            Func<string, IAsyncFileSource> fileSystemSourceFactory,
            Func<IAsyncFileSource, IAsyncFileSource, IStreamCopy> streamCopyFactory)
        {
            _httpFileSourceFactory = httpFileSourceFactory;
            _fileSystemSourceFactory = fileSystemSourceFactory;
            _streamCopyFactory = streamCopyFactory;
        }

        public async Task<CovertArtDownloader> Create(string url, string localPath) 
        {
            IAsyncFileSource source = _httpFileSourceFactory(url);

            IAsyncFileSource destination = _fileSystemSourceFactory(localPath);

            IStreamCopy copier = _streamCopyFactory(source, destination);

            CovertArtDownloader downloader = new CovertArtDownloader(copier);

            return await Task.FromResult(downloader);
        }
    }
}
