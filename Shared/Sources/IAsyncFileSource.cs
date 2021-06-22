using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Sources
{
    public interface IAsyncFileSource
    {
        Task<Stream> GetStreamAsync(CancellationToken cancellationToken);
    }
}
