using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Sources
{
    public class MemorySource : IFileSource, IAsyncFileSource, IDisposable, IAsyncDisposable
    {
        private readonly MemoryStream _stream;

        public MemorySource(byte[] buffer)
        {
            _stream = new MemoryStream(buffer);
        }

        public Stream GetStream()
        {
            return _stream;
        }

        public async Task<Stream> GetStreamAsync(CancellationToken cancellationToken)
        {
            return await Task.FromResult(_stream);
        }

        public void Dispose()
        {
            _stream.Dispose();
        }

        public async ValueTask DisposeAsync()
        {
            await _stream.DisposeAsync();
        }
    }
}
