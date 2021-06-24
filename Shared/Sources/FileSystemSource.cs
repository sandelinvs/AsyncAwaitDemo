using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Sources
{
    public class FileSystemSource : IAsyncFileSource
    {
        private readonly FileStream _fileStream;

        private bool _disposed;

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
            if (_disposed)
                return;

            _fileStream.Dispose();

            _disposed = true;
        }

        public async ValueTask DisposeAsync()
        {
            if (_disposed)
            {
                await Task.CompletedTask;
            }
            else 
            {
                _disposed = true;

                await _fileStream.DisposeAsync();
            }
        }
    }
}
