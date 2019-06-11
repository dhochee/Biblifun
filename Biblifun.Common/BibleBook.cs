using System.Collections.Generic;

namespace Biblifun.Common
{
    public class BibleBook
    {
        public int BookId { get; set; }

        public List<string> Names { get; set; }

        public List<BibleChapter> Chapters { get; set; }

        public bool IsSingleChapter
        { 
            get
            {
                return this.Chapters.Count == 1;
            }
        }

        public string Language { get; set; }
    }
}
