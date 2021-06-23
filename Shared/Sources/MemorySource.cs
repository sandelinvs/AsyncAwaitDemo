using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Sources
{
    public class MemorySource : IAsyncFileSource
    {
        private readonly MemoryStream _stream;

        public MemorySource(byte[] buffer)
        {
            _stream = new MemoryStream(buffer);
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
