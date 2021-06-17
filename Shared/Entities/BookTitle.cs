using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class BookTitle
    {
        public string Title { get; set; }

        public string TitleUrl { get; set; }

        public string Author { get; set; }

        public string AuthorUrl { get; set; }

        public string CoverArtUrl { get; set; }

        public override string ToString()
        {
            return Title + " by " + Author;
        }
    }
}
