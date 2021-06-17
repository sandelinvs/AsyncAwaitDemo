using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using HtmlAgilityPack;

namespace Shared
{
    public class BookTitle
    {
        public string Title { get; set; }

        public string TitleUrl { get; set; }

        public string Author { get; set; }

        public string AuthorUrl { get; set; }

        public string CoverArtUrl { get; set; }
    }

    public interface IBookListParser
    {
        IEnumerable<BookTitle> Parse(string html);
    }

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

    internal static class Helpers
    {
        public static string GetCoverArtUrl(this HtmlNode article)
        {
            var covertArt = (from img in article.Descendants("img")
                             where img.GetClasses().Contains("img-responsive")
                             select img.GetAttributeValue("src", ""))
                             .FirstOrDefault();

            var decoded = HttpUtility.HtmlDecode(covertArt);

            return decoded;
        }

        public static (string bookTitle, string bookUrl) GetBookInfo(this HtmlNode article)
        {
            var title = (from a in article.Descendants("a")
                         where a.ParentNode.Name == "h2"
                         select new
                         {
                             BookTitle = a.InnerText.Trim(),
                             BookUrl = HttpUtility.HtmlDecode(a.GetAttributeValue("href", ""))
                         }).FirstOrDefault();

            return (title.BookTitle, title.BookUrl);
        }

        public static (string authorName, string authorUrl) GetAuthorInfo(this HtmlNode article)
        {
            var author = (from a in article.Descendants("a")
                          where a.ParentNode.Name == "div" && a.ParentNode.GetClasses().Contains("author")
                          select new
                          {
                              AuthorName = a.InnerText.Trim(),
                              AuthorUrl = HttpUtility.HtmlDecode(a.GetAttributeValue("href", ""))
                          }).FirstOrDefault();

            return (author.AuthorName, author.AuthorUrl);
        }
    }
}
