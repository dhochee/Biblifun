namespace Biblifun.Common
{
    /// <summary>
    /// Describes a verse or sequence of verses with a specfic book, chapter, and verse. 
    /// A string representation of this may be obtained using the Code property.
    /// </summary>
    public class VerseSetDescriptor
    {
        /// <summary>
        /// The book number id.
        /// </summary>
        public int BookId { get; set; }

        /// <summary>
        /// The chapter number.
        /// </summary>
        public int Chapter { get; set; }

        /// <summary>
        /// The starting verse number of the set.
        /// </summary>
        public int Start { get; set; }


        /// <summary>
        /// The ending verse number of the set. The same as Start if the set is a single verse.
        /// </summary>
        public int End { get; set; }

        /// <summary>
        /// Indicates if the book has a single chapter. Including this metadata helps avoid looking it up elsewhere.
        /// </summary>
        public bool IsSingleChapterBook { get; set; }

        /// <summary>
        /// Returns a fixed length string representing the current 
        /// VerseSetDescriptor which identifies either a single verse or 
        /// sequence of verses from a single book and chapter.
        /// 
        /// The format of a verse set code is: BBCCCSSSEEE, where the B 
        /// digits identify the book, the C digits identify the chapter, and 
        /// the S and E digits identify the start and end verses of the set.
        /// 
        /// Example: "40024013014" = B:40 C:024 S:013 E:014 = Matthew 24:13,14        
        /// </summary>
        public string Code
        {
            get
            {
                var end = Start == End ? "" : $"{End:D3}";

                return $"{BookId:D2}{Chapter:D3}{Start:D3}{end}";
            }
        }

        /// <summary>
        /// Given a string identifier code for a specific verse or sequence of 
        /// verses, return a corresponding VerseSetDescriptor.
        /// 
        /// Example: "40024013014" returns the VerseSetDescriptor for Matthew 24:13,14.
        /// </summary>
        public static VerseSetDescriptor FromCode(string verseSetCode)
        {
            VerseSetDescriptor descriptor = null;

            if (int.TryParse(verseSetCode.Substring(0, 2), out int bookId))
            {
                if (int.TryParse(verseSetCode.Substring(2, 3), out int chapter))
                {
                    if (int.TryParse(verseSetCode.Substring(5, 3), out int start))
                    {
                        var end = start;

                        if (verseSetCode.Length == 11)
                        {
                            int.TryParse(verseSetCode.Substring(8, 3), out end);
                        }

                        descriptor = new VerseSetDescriptor
                        {
                            BookId = bookId,
                            Chapter = chapter,
                            Start = start,
                            End = end
                        };
                    }
                }
            }

            return descriptor;
        }
    }
}
