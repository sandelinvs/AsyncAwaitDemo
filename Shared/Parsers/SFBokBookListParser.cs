using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;

namespace Shared
{
    public class SFBokBookListParser : IBookListParser
    {
        public IEnumerable<BookTitle> Parse(string html)
        {
            var doc = new HtmlDocument();

            doc.LoadHtml(html);

            var titles = from article in doc.DocumentNode.Descendants("article")
                         let @class = article.GetAttributeValue("class", "")
                         where @class.Contains("volume")
                         let covertArtUrl = article.ToCoverArtUrl()
                         let titleInfo = article.ToBookInfo()
                         let authorInfo = article.ToAuthorInfo()
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
