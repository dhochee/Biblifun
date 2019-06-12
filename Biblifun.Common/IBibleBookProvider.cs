using System.Collections.Generic;

namespace Biblifun.Common
{
    public interface IBibleBookProvider
    {
        List<BibleBook> BibleBooks { get; }

        BibleBook GetBookById(int bookId);
    }
}