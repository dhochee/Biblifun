using Biblifun.Common.Util;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Biblifun.Data.Models
{
    public class BibleBook
    {
        public BibleBook()
        {
            Chapters = new List<BibleChapter>();
            BookNames = new List<BibleBookName>();
        }

        public int BibleBookId { get; set; }

        public string Name { get; set; }

        public int TotalChapters { get; set; }


        public ICollection<BibleChapter> Chapters { get; set; }

        public ICollection<BibleBookName> BookNames { get; set; }

        /// <summary>
        /// Returns the display name of the Bible book given the specified language. 
        /// This name may contain non-breaking spaces for display purposes.
        /// </summary>
        public string GetNameForLanguage(string language)
        {
            return this.BookNames.FirstOrDefault(bn => bn.Language == language)?.Name;
        }

        /// <summary>
        /// Returns a consolidated list of all identifiers which may be used for matching the
        /// Bible book in the specified language. This includes the standard name and abbreviations 
        /// with and without diacritics. Non-breaking spaces are replaced with breaking spaces.
        /// </summary>
        public List<string> GetAllIdentifiersForLanguage(string language)
        {
            List<string> allNames = null;

            var bookNames = this.BookNames.FirstOrDefault(bn => bn.Language == language);

            if(bookNames != null)
            {
                var altNames = bookNames.AlternateIdentifiers.ReplaceNonBreakingSpaces();

                if (!string.IsNullOrWhiteSpace(altNames))
                {
                    allNames = altNames.Split(',').ToList();
                }

                allNames.Insert(0, bookNames.Name.ReplaceNonBreakingSpaces());
            }

            return allNames;
        }

        public bool IsSingleChapter
        {
            get
            {
                return this.Chapters.Count == 1;
            }
        }


    }
}
