using System.Collections.Generic;
using System.Linq;

namespace Biblifun.Common
{
    public class BibleBookProvider : IBibleBookProvider
    {
        readonly ILanguageProvider _languageProvider;

        public BibleBookProvider(ILanguageProvider languageProvider)
        {
            _languageProvider = languageProvider;
        }

        public List<BibleBook> BibleBooks
        {
            get
            {
                //TODO: Populate from database

                var allBooks = new List<BibleBook>();

                return allBooks.Where(b => b.Language == _languageProvider.Language).ToList();
            }
        }

    }
}
