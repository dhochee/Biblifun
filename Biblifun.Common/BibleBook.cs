using System.Collections.Generic;

namespace Biblifun.Common
{
    public class BibleBook
    {
        public int BookId { get; set; }

        public string Name { get; set; }

        public List<string> AlternativeNames { get; set; }

        public List<string> AllNames
        {
            get
            {
                var allNames = new List<string>(AlternativeNames);
                allNames.Insert(0, Name);

                return allNames;
            }
        }

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
