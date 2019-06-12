using Biblifun.Common;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using StringTokenFormatter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
        /// </summary>
        /// <param name="verseSetId"></param>
        /// <returns></returns>
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

            var settings = _languageSettingsProvider.GetLanguageSettings(_languageProvider.Language);

            var url = settings.ScriptureLookupUrlTemplate.FormatToken(tokens);

            return url;
        }

        private string CleanVerseHtml(HtmlNode verseNode)
        {
            ReplaceHtmlTag(ref verseNode, "//a[contains(@class, 'vl')]", "span", "vl");

            RemoveUnwantedHtmlTags(ref verseNode, new List<string>() { "a" });

            var allChildNodes = verseNode.SelectNodes("//*");

            foreach(var node in allChildNodes)
            {
                node.Attributes.Remove("id");
                node.Attributes.Remove("data-pid");
            }

            return verseNode.InnerHtml.Replace("+", "").Replace("*","").Trim();
        }

        public static void ReplaceHtmlTag(ref HtmlNode htmlNode, string elementSelector, string replacementTag, string className)
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
        public static void RemoveUnwantedHtmlTags(ref HtmlNode htmlNode, List<string> unwantedTags)
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
