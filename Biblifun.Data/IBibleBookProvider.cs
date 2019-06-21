using Biblifun.Data.Models;
using System.Collections.Generic;

namespace Biblifun.Data
{
    public interface IBibleBookProvider
    {
        string Language { get; set;  }

        List<BibleBook> BibleBooks { get; }

        BibleBook GetBookById(int bookId);
    }
}