using System;
using System.Collections.Generic;
using System.Linq;
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


        public string GetNameForLanguage(string language)
        {
            return this.BookNames.FirstOrDefault(bn => bn.Language == language)?.Name;
        }

        public List<string> GetAbbreviationsForLanguage(string language)
        {
            List<string> abbreviationsList = null;

            var abbreviations = this.BookNames.FirstOrDefault(bn => bn.Language == language)?.Abbreviations;

            if(!string.IsNullOrWhiteSpace(abbreviations))
            {
                abbreviationsList = abbreviations.Split(',').ToList();
            }

            return abbreviationsList;
        }

    }
}
