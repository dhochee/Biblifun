using Biblifun.Common;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using StringTokenFormatter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblifun.WebLookup
{
    public class VerseRetriever : IVerseRetriever
    {
        readonly ILanguageProvider _languageProvider;
        readonly ILanguageSettingsProvider _languageSettingsProvider;
        readonly IVerseParser _verseParser;
        readonly ILogger<VerseRetriever> _logger;

        public VerseRetriever(ILanguageProvider languageProvider,
                              ILanguageSettingsProvider languageSettingsProvider,
                              IVerseParser verseParser,
                              ILogger<VerseRetriever> logger)
        {
            _languageProvider = languageProvider;
            _languageSettingsProvider = languageSettingsProvider;
            _verseParser = verseParser;
            _logger = logger;
        }

        /// <summary>
        /// Given a verse set identifying a single verse or sequence of verses from a 
        /// single chapter, asynchronously retrieve the language-specific HTML from the 
        /// WOL website, modifying the returned HTML in the process to suit our use.
        /// </summary>
        public async Task<string> GetVerseHtmlAsync(IVerseSetId verseSetId)
        {
            string verseHtml = null;

            var url = GetVerseUrl(verseSetId);

            try
            {
                var webClient = new HtmlWeb();

                var document = await webClient.LoadFromWebAsync(url);

                var articleNode = document.DocumentNode.SelectSingleNode("//li[contains(@class,'bibleCitation')]/article");

                if(!string.IsNullOrWhiteSpace(articleNode?.InnerHtml))
                {
                    verseHtml = CleanVerseHtml(articleNode);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving HTML for \"{verseSetId}\".");
            }

            return verseHtml;
        }

        /// <summary>
        /// Given a verse set id, get the url that will retrieve the 
        /// verse from the WOL site using the configured language.
        /// </summary>
        public string GetVerseUrl(IVerseSetId verseSetId)
        {
            var bookName = _verseParser.GetBookNameById(verseSetId.BookId);

            var isSingleChapter = _verseParser.IsSingleChapterBook(verseSetId.BookId);

            var tokens = new Dictionary<string, string>
            {
                { "book",  bookName },
                { "chapter", isSingleChapter ? "" : $"{verseSetId.Chapter}:" },
                { "verseStart", verseSetId.Start.ToString() },
                { "verseEnd", verseSetId.Start == verseSetId.End ? "" : $"-{verseSetId.End}" }
            };

            // get the URL template specified for the language
            var settings = _languageSettingsProvider.GetLanguageSettings(_languageProvider.Language);

            // replace the tokens in the template with the values we obtained above
            var url = settings.ScriptureLookupUrlTemplate.FormatToken(tokens);

            return url;
        }

        /// <summary>
        /// Given the HTML node for the retrieved verses, clean up for our use with the following changes:
        /// 
        /// 1. Replace verse links with spans containing the same content. Add our own class to identify the verse number.
        /// 2. Remove other links but preserve the content, in case there's any content that could need preserving.
        /// 3. Remove all "id" and "data-pid" attributes.
        /// 4. Remove reference and footnote character indicators.
        /// </summary>
        private string CleanVerseHtml(HtmlNode verseNode)
        {
            ReplaceHtmlTag(ref verseNode, "//a[contains(@class, 'vl')]", "span", "verseNumber");

            RemoveUnwantedHtmlTags(ref verseNode, new List<string>() { "a" });

            var allChildNodes = verseNode.SelectNodes("//*");

            foreach(var node in allChildNodes)
            {
                node.Attributes.Remove("id");
                node.Attributes.Remove("data-pid");
            }

            return verseNode.InnerHtml.Replace("+", "").Replace("*","").Trim();
        }

        /// <summary>
        /// Replace the elements found with the specified selector with the replacement element, 
        /// preserving the content, and optionally applying the specified class name.
        /// </summary>
        private static void ReplaceHtmlTag(ref HtmlNode htmlNode, string elementSelector, string replacementTag, string className)
        {
            HtmlNodeCollection tryGetNodes = htmlNode.SelectNodes(elementSelector);

            if (tryGetNodes == null || !tryGetNodes.Any())
            {
                return;
            }

            foreach (HtmlNode node in tryGetNodes)
            {
                var classString = className != null ? $" class={className}" : "";
                var replacementNode = HtmlNode.CreateNode($"<{replacementTag}{classString}>{node.InnerHtml}</{replacementTag}>");
                node.ParentNode.ReplaceChild(replacementNode, node);
            }
        }

        // adapted from source: https://stackoverflow.com/a/28298882/773798
        /// <summary>
        /// Remove the specified unwanted tags while preserving their inner content.
        /// </summary>
        private static void RemoveUnwantedHtmlTags(ref HtmlNode htmlNode, List<string> unwantedTags)
        {

            HtmlNodeCollection tryGetNodes = htmlNode.SelectNodes("./*|./text()");

            if (tryGetNodes == null || !tryGetNodes.Any())
            {
                return;
            }

            var nodes = new Queue<HtmlNode>(tryGetNodes);

            while (nodes.Count > 0)
            {
                var node = nodes.Dequeue();
                var parentNode = node.ParentNode;

                var childNodes = node.SelectNodes("./*|./text()");

                if (childNodes != null)
                {
                    foreach (var child in childNodes)
                    {
                        nodes.Enqueue(child);
                    }
                }

                if (unwantedTags.Any(tag => tag == node.Name))
                {
                    if (childNodes != null)
                    {
                        foreach (var child in childNodes)
                        {
                            parentNode.InsertBefore(child, node);
                        }
                    }

                    parentNode.RemoveChild(node);

                }
            }
        }
    }
}
