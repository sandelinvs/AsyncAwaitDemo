using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Sources
{
    public sealed class FileSystemSource : IAsyncFileSource, IDisposable, IAsyncDisposable
    {
        private readonly FileStream _fileStream;

        public FileSystemSource(string fileName)
        {
            _fileStream = new FileStream(fileName, 
                FileMode.OpenOrCreate, 
                FileAccess.Write,
                FileShare.None,
                4096, 
                FileOptions.Asynchronous);
        }

        public async Task<Stream> GetStreamAsync(CancellationToken cancellation = default)
        {
            return await Task.FromResult(_fileStream);
        }

        public void Dispose()
        {
            _fileStream.Dispose();
        }

        public async ValueTask DisposeAsync()
        {
            await _fileStream.DisposeAsync();
        }
    }
}
