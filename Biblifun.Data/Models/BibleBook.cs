using System;
using System.Collections.Generic;
using System.Text;

namespace Biblifun.Data.Models
{
    public class BibleBook
    {
        public int BibleBookId { get; set; }

        public string Name { get; set; }

        public int TotalChapters { get; set; }


        public ICollection<BibleChapter> Chapters { get; set; }

        public ICollection<BibleBookName> BookNames { get; set; }

    }
}
