using System;
using System.Collections.Generic;
using System.Text;

namespace Biblifun.Data.Models
{
    public class BibleChapter
    {
        public int BibleChapterId { get; set; }

        
        public int BibleBookId { get; set; }

        public BibleBook Book { get; set; }


        public int ChapterNumber { get; set; }

        public int TotalVerses { get; set; }
    }
}
