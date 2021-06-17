﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shared
{
    public interface IBatchedDownloader
    {
        Task<IEnumerable<Task<IEnumerable<string>>>> Download(params string[] urls);
    }
}