using Biblifun.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace Biblifun.Data
{
    public class BibleBookProvider : IBibleBookProvider
    {
        readonly ApplicationDbContext _dbContext;
        readonly ILanguageProvider _languageProvider;

        public BibleBookProvider(ApplicationDbContext dbContext,
                                 ILanguageProvider languageProvider)
        {

            _dbContext = dbContext;
            _languageProvider = languageProvider;
        }

        public List<BibleBook> BibleBooks
        {
            get
            {
                return _dbContext.BibleBooks.ToList();
            }
        }

        public string Language { get; set; }

        public string CurrentLanguage
        {
            get
            {
                return this.Language ?? _languageProvider.Language ?? "en";
            }
        }

        public BibleBook GetBookById(int bookId)
        {
            return _dbContext.BibleBooks.FirstOrDefault(bb => bb.BibleBookId == bookId);
        }
    }
}
