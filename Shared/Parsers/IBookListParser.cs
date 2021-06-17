using System.Collections.Generic;

namespace Shared
{
    public interface IBookListParser
    {
        IEnumerable<BookTitle> Parse(string html);
    }
}
