using Biblifun.Data;
using Biblifun.Data.Repositories.Interfaces;
using Biblifun.WebLookup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Biblifun.Website.Managers
{
    public class ScriptureLookupManager : IScriptureLookupManager
    {
        readonly IVerseParser _verseParser;
        readonly Func<string, IVerseRetriever> _webRetrieverFactory;
        readonly IVerseCacheRepository _verseCacheRepository;

        public ScriptureLookupManager(IVerseParser verseParser,
                                      Func<string, IVerseRetriever> webRetrieverFactory,
                                      IVerseCacheRepository verseCacheRepository)
        {
            _verseParser = verseParser;
            _webRetrieverFactory = webRetrieverFactory;
            _verseCacheRepository = verseCacheRepository;
        }

        public async Task<string> GetVerseHtmlFromSetCode(string verseCode, string language)
        {
            // first check the cache
            string html = null;

            html = _verseCacheRepository.GetVerseHtmlByCode(verseCode);

            if (html == null)
            {
                var webRetriever = _webRetrieverFactory(language);

                html = await webRetriever.GetVerseHtmlAsync(VerseSetDescriptor.FromCode(verseCode), language);

                if(html != null)
                {
                    _verseCacheRepository.AddVerseHtml(verseCode, language, html);
                }
            }

            return html;
        }
    }
}
