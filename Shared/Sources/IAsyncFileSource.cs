using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Sources
{
    public interface IAsyncFileSource : IDisposable, IAsyncDisposable
    {
        Task<Stream> GetStreamAsync(CancellationToken cancellationToken);
    }
}
