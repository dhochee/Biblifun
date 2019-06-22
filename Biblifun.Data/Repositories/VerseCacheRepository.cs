using Biblifun.Data.Models;
using Biblifun.Data.Repositories.Interfaces;
using System.Linq;

namespace Biblifun.Data.Repositories
{
    public class VerseCacheRepository : Repository<VerseCache>, IVerseCacheRepository
    {
        public VerseCacheRepository(ApplicationDbContext context) : base(context)
        { }


        public string GetVerseHtmlByCode(string verseCode)
        {
            return _appContext.CachedVerses.FirstOrDefault(v => v.VerseSetCode == verseCode)?.Html;
        }

        public void AddVerseHtml(string verseCode, string language, string html)
        {
            _appContext.CachedVerses.Add(new VerseCache
            {
                VerseSetCode = verseCode,
                Language = language,
                Html = html
            });
        }

        private ApplicationDbContext _appContext => (ApplicationDbContext)_context;
    }
}
