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
        readonly IVerseRetriever _webRetriever;
        ISearchCountRetriever _searchCountRetriever;
        readonly IUnitOfWork _unitOfWork;

        public ScriptureLookupManager(IVerseRetriever webRetriever,
                                      ISearchCountRetriever searchCountRetriever,
                                      IUnitOfWork unitOfWork)
        {
            _webRetriever = webRetriever;
            _searchCountRetriever = searchCountRetriever;
            _unitOfWork = unitOfWork;
        }

        public async Task<string> GetVerseHtmlFromSetCode(string verseCode, string language)
        {
            // first check the cache
            string html = _unitOfWork.VerseCacheRepo.GetVerseHtmlByCode(verseCode, language);

            // if not cached, use the web retriever
            if (html == null)
            {
                var verseDescriptor = VerseSetDescriptor.FromCode(verseCode);

                html = await _webRetriever.GetVerseHtmlAsync(verseDescriptor, language);

                if(html != null)
                {
                    // do we already have a count from another language?
                    var searchCount = _unitOfWork.VerseCacheRepo.GetSearchCountByCode(verseCode);

                    if(searchCount == null)
                    {
                        searchCount = await _searchCountRetriever.GetSearchCountAsync(verseDescriptor);
                    }

                    _unitOfWork.VerseCacheRepo.AddVerseHtml(verseCode, language, html, searchCount ?? 0);

                    _unitOfWork.SaveChanges();
                }
            }

            return html;
        }
    }
}
