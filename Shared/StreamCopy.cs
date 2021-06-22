using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Sources
{

    public interface IStreamCopy
    {
        Task Copy(CancellationToken cancellationToken);
    }

    public class StreamCopy : IStreamCopy
    {
        private readonly IAsyncFileSource _source;
        private readonly IAsyncFileSource _destination;

        public StreamCopy(IAsyncFileSource source, IAsyncFileSource destination)
        {
            _source = source;
            _destination = destination;
        }

        public async Task Copy(CancellationToken cancellationToken = default)
        {
            await using Stream src = await _source.GetStreamAsync(cancellationToken);

            await using Stream dst = await _destination.GetStreamAsync(cancellationToken);

            await src.CopyToAsync(dst, cancellationToken);
        }
    }
}
