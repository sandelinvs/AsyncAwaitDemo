using System;
using System.Linq;
using System.Web;
using HtmlAgilityPack;

namespace Shared
{
    internal static class SFBokHelpers
    {
        public static string ToCoverArtUrl(this HtmlNode article)
        {
            var covertArt = (from img in article.Descendants("img")
                             where img.GetClasses().Contains("img-responsive")
                             select img.GetAttributeValue("src", ""))
                             .FirstOrDefault();

            var decoded = HttpUtility.HtmlDecode(covertArt);

            return decoded;
        }

        public static (string bookTitle, string bookUrl) ToBookInfo(this HtmlNode article)
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

        public static (string authorName, string authorUrl) ToAuthorInfo(this HtmlNode article)
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
