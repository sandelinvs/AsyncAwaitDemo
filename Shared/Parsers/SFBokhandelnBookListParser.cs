using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace Shared
{
    public class SFBokhandelnBookListParser : IBookListParser
    {
        public IEnumerable<BookTitle> Parse(string html)
        {
            var doc = new HtmlDocument();

            doc.LoadHtml(html);

            var titles = from article in doc.DocumentNode.Descendants("article")
                         where article.Attributes.Any(a => a.Name == "class")
                         let @class = article.GetAttributeValue("class", null)
                         where @class.Contains("volume")
                         let covertArtUrl = article.GetCoverArtUrl()
                         let titleInfo = article.GetBookInfo()
                         let authorInfo = article.GetAuthorInfo()
                         select new BookTitle
                         {
                             Author = authorInfo.authorName,
                             AuthorUrl = authorInfo.authorUrl,
                             Title = titleInfo.bookTitle,
                             TitleUrl = titleInfo.bookUrl,
                             CoverArtUrl = covertArtUrl
                         };

            return titles.ToList();
        }

        

        

        

        
    }
}
