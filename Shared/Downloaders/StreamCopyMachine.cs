using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Shared.Sources
{

    public interface IStreamCopyMachine
    {
        Task Save(CancellationToken cancellationToken);
    }

    public class StreamCopyMachine : IStreamCopyMachine
    {
        private readonly IAsyncFileSource _source;
        private readonly IAsyncFileSource _destination;

        public StreamCopyMachine(IAsyncFileSource source, IAsyncFileSource destination)
        {
            _source = source;
            _destination = destination;
        }

        public async Task Save(CancellationToken cancellationToken)
        {
            Stream src = await _source.GetStreamAsync(cancellationToken);

            Stream dst = await _destination.GetStreamAsync(cancellationToken);

            await src.CopyToAsync(dst, cancellationToken);
        }
    }
}
