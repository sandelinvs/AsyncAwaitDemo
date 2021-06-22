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
                         let bookTitle = a.InnerText.Trim()
                         let bookUrl = HttpUtility.HtmlDecode(a.GetAttributeValue("href", ""))
                         select (bookTitle, bookUrl))
                         .FirstOrDefault();

            return title;
        }

        public static (string authorName, string authorUrl) ToAuthorInfo(this HtmlNode article)
        {
            var author = (from a in article.Descendants("a")
                          where a.ParentNode.Name == "div" && a.ParentNode.GetClasses().Contains("author")
                          let authorName = a.InnerText.Trim()
                          let authorUrl = HttpUtility.HtmlDecode(a.GetAttributeValue("href", ""))
                          select (authorName, authorUrl))
                          .FirstOrDefault();

            return author;
        }
    }
}
