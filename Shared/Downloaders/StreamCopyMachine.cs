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
        private readonly IAsyncFileSource _asyncSource;
        private readonly IFileSource _source;

        private readonly IAsyncFileSource _asyncDestination;
        private readonly IFileSource _destination;

        public StreamCopyMachine(IAsyncFileSource source, IFileSource destination)
        {
            _asyncSource = source;
            _destination = destination;
        }

        public async Task Save(CancellationToken cancellationToken)
        {
            Stream src = await _asyncSource.GetStreamAsync(cancellationToken);

            Stream dst = _destination.GetStream();

            await src.CopyToAsync(dst, cancellationToken);
        }
    }
}
