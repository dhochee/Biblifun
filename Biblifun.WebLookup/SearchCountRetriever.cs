using Biblifun.Common;
using Biblifun.Common.Util;
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
    /// Used to retrieve the reference count  NWT scripture text from the Watchtower Online Library website (https://wol.jw.org).
    /// </summary>
    public class SearchCountRetriever : ISearchCountRetriever
    {
        readonly IVerseParser _verseParser;
        readonly ILogger<SearchCountRetriever> _logger;

        public SearchCountRetriever(IVerseParser verseParser,
                                    ILogger<SearchCountRetriever> logger)
        {
            _verseParser = verseParser;
            _logger = logger;
        }


        /// <summary>
        /// Given a verse set id, get the url that will retrieve the 
        /// verse from the WOL site using the configured language.
        /// </summary>
        public string GetSearchCountUrl(VerseSetDescriptor setDescriptor)
        {
            var bookName = _verseParser.GetBookNameById(setDescriptor.BookId);

            var tokens = new Dictionary<string, string>
            {
                { "book",  bookName },
                { "chapter", setDescriptor.IsSingleChapterBook ? "" : $"{setDescriptor.Chapter}:" },
                { "verseStart", setDescriptor.Start.ToString() },
                { "verseEnd", setDescriptor.Start == setDescriptor.End ? "" : $"-{setDescriptor.End}" }
            };

            // replace the tokens in the template with the values we obtained above
            var url = Settings.Default.SearchCountUrlTemplate.FormatToken(tokens);

            return url;
        }


        public async Task<int?> GetSearchCountAsync(VerseSetDescriptor setDescriptor)
        {
            int? searchCount = null;

            var url = GetSearchCountUrl(setDescriptor);

            try
            {
                var webClient = new HtmlWeb();

                var document = await webClient.LoadFromWebAsync(url);

                var resultCountNode = document.DocumentNode.SelectSingleNode("//span[@id='resultsCount']/span");

                if (!string.IsNullOrWhiteSpace(resultCountNode?.InnerText))
                {
                    var searchCountStr = Numbers.GetNumberFromStr(resultCountNode?.InnerText);

                    if(int.TryParse(searchCountStr, out int count));
                    {
                        searchCount = count;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving search count for \"{setDescriptor.Code}\".");
            }

            return searchCount;
        }
    }
}
