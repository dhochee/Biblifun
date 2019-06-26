using Biblifun.Data.Models;
using Biblifun.Data.Repositories.Interfaces;
using System.Linq;

namespace Biblifun.Data.Repositories
{
    public class VerseCacheRepository : Repository<VerseCache>, IVerseCacheRepository
    {
        public VerseCacheRepository(ApplicationDbContext context) : base(context)
        { }


        public string GetVerseHtmlByCode(string verseCode, string language)
        {
            return _appContext.CachedVerses.FirstOrDefault(v => v.VerseSetCode == verseCode && v.Language == language)?.Html;
        }

        public void AddVerseHtml(string verseCode, string language, string html, int searchCount)
        {
            _appContext.CachedVerses.Add(new VerseCache
            {
                VerseSetCode = verseCode,
                Language = language,
                Html = html,
                SearchCount = searchCount
            });
        }

        public int? GetSearchCountByCode(string verseCode)
        {
            return _appContext.CachedVerses
                              .Where(v => v.VerseSetCode == verseCode && v.SearchCount > 0)
                              .Select(v => (int?)v.SearchCount)
                              .FirstOrDefault();
        }

        private ApplicationDbContext _appContext => (ApplicationDbContext)_context;
    }
}
