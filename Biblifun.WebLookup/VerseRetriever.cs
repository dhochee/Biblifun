using Biblifun.Data;
using Biblifun.WebLookup.Util;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using StringTokenFormatter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblifun.WebLookup
{
    /// <summary>
    /// Used to retrieve NWT scripture text from the Watchtower Online Library website (https://wol.jw.org).
    /// </summary>
    public class VerseRetriever : IVerseRetriever
    {
        readonly ILanguageSettingsProvider _languageSettingsProvider;
        readonly IVerseParser _verseParser;
        readonly ILogger<VerseRetriever> _logger;

        public VerseRetriever(ILanguageSettingsProvider languageSettingsProvider, 
                              IVerseParser verseParser,
                              ILogger<VerseRetriever> logger)
        {
            _languageSettingsProvider = languageSettingsProvider;
            _verseParser = verseParser;
            _logger = logger;
        }

        /// <summary>
        /// Given a verse set identifying a single verse or sequence of verses from a 
        /// single chapter, asynchronously retrieve the language-specific HTML from the 
        /// WOL website, modifying the returned HTML in the process to suit our use.
        /// </summary>
        public async Task<string> GetVerseHtmlAsync(VerseSetDescriptor verseSet, string language)
        {
            string verseHtml = null;

            var url = GetVerseUrl(verseSet, language);

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
                _logger.LogError(ex, $"Error retrieving HTML for \"{verseSet.Code}\".");
            }

            return verseHtml;
        }

        /// <summary>
        /// Given a verse set id, get the url that will retrieve the 
        /// verse from the WOL site using the configured language.
        /// </summary>
        public string GetVerseUrl(VerseSetDescriptor setDescriptor, string language)
        {
            var bookName = _verseParser.GetBookNameById(setDescriptor.BookId, language);

            var tokens = new Dictionary<string, string>
            {
                { "book",  bookName },
                { "chapter", setDescriptor.IsSingleChapterBook ? "" : $"{setDescriptor.Chapter}:" },
                { "verseStart", setDescriptor.Start.ToString() },
                { "verseEnd", setDescriptor.Start == setDescriptor.End ? "" : $"-{setDescriptor.End}" }
            };

            // get the URL template specified for the language
            var settings = _languageSettingsProvider.GetLanguageSettings(language);

            // replace the tokens in the template with the values we obtained above
            var url = settings.ScriptureLookupUrlTemplate.FormatToken(tokens);

            return url;
        }

        /// <summary>
        /// Given the HTML article node for the retrieved verses, clean up for our use with the following changes:
        /// 
        /// 1. Replace verse links with spans containing the same content (with our own class to identify the verse number).
        /// 2. Remove other links but preserve the content, in case there's any content that could need preserving.
        /// 3. Remove all "id" and "data-pid" attributes.
        /// 4. Remove reference and footnote character indicators.
        /// 5. Pretty-print format the HTML.
        /// </summary>
        private string CleanVerseHtml(HtmlNode verseNode)
        {
            // 1. Replace verse links with spans containing the same content (with our own class to identify the verse number).
            ReplaceHtmlTag(ref verseNode, "//a[contains(@class, 'vl')]", "span", "verseNumber");

            // 2. Remove other links but preserve the content, in case there's any content that could need preserving.
            RemoveUnwantedHtmlTags(ref verseNode, new List<string>() { "a" });

            // 3. Remove all "id" and "data-pid" attributes.
            var allChildNodes = verseNode.SelectNodes("//*");

            foreach(var node in allChildNodes)
            {
                node.Attributes.Remove("id");
                node.Attributes.Remove("data-pid");
            }

            // 4. Remove reference and footnote character indicators.
            verseNode.InnerHtml = verseNode.InnerHtml.Replace("+", "").Replace("*", "").Trim();

            // 5. Pretty-print format the HTML of the child nodes (not required, 
            //    but favoring developer readability over trivial space usage).
            string html = verseNode.ChildNodes.ToFormattedHtml();

            return html;
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
