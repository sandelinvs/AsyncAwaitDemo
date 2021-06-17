using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared
{
    public interface IBatchedDownloader
    {
        Task<IEnumerable<Task<IEnumerable<string>>>> Download(params string[] urls);
    }

    public class BatchedDownloader : IBatchedDownloader
    {
        public async Task<IEnumerable<Task<IEnumerable<string>>>> Download(params string[] urls)
        {
            throw new NotImplementedException();
        }
    }
}