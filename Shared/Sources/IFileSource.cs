using System;
using System.IO;

namespace Shared.Sources
{
    public interface IFileSource
    {
        Stream GetStream();
    }
}
