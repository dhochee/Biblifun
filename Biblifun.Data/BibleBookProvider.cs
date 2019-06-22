using Biblifun.Data.Models;
using System.Collections.Generic;
using System.Linq;

namespace Biblifun.Data
{
    public class BibleBookProvider : IBibleBookProvider
    {
        private const string DEFAULT_LANGUAGE = "en";

        readonly ApplicationDbContext _dbContext;

        public BibleBookProvider(ApplicationDbContext dbContext)
        {

            _dbContext = dbContext;
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
                return this.Language ?? DEFAULT_LANGUAGE;
            }
        }

        public BibleBook GetBookById(int bookId)
        {
            return _dbContext.BibleBooks.FirstOrDefault(bb => bb.BibleBookId == bookId);
        }
    }
}
