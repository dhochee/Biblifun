using System.Collections.Generic;
using System.Linq;

namespace Biblifun.Common
{
    public class BibleBookProvider : IBibleBookProvider
    {
        readonly ILanguageProvider _languageProvider;

        private List<BibleBook> _allBooks;
        private Dictionary<string, Dictionary<int, BibleBook>> _bookNamesById;

        public BibleBookProvider(ILanguageProvider languageProvider)
        {
            //TODO: Inject Bible Books repository

            _languageProvider = languageProvider;


            _allBooks = new List<BibleBook>();
            _bookNamesById = new Dictionary<string, Dictionary<int, BibleBook>>();
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

        public BibleBook GetBookById(int bookId)
        {
            // TODO: Lookup in database    

            return _bookNamesById[_languageProvider.Language][bookId];
        }
    }
}
