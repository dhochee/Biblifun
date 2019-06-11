using System.Collections.Generic;
using System.Linq;

namespace Biblifun.Common
{
    public class BibleBookProvider : IBibleBookProvider
    {
        readonly ILanguageProvider _languageProvider;

        public BibleBookProvider(ILanguageProvider languageProvider)
        {
            _languageProvider = languageProvider;
        }

        public List<BibleBook> GetBibleBooks()
        {
            //TODO: Populate from database

            var allBooks = new List<BibleBook>()
            {
                new BibleBook
                {
                    BookId = 1,
                    Names = new List<string>() { "Genesis", "Gen", "Ge" },
                    Language = "en",
                    Chapters = new List<BibleChapter>()
                },
                new BibleBook
                {
                    BookId = 2,
                    Names = new List<string>() { "Exodus", "Exo", "Ex" },
                    Language = "en",
                    Chapters = new List<BibleChapter>()
                },
                new BibleBook
                {
                    BookId = 40,
                    Names = new List<string>() { "Matthew", "Matt", "Mat", "Ma" },
                    Language = "en",
                    Chapters = new List<BibleChapter>()
                },
                new BibleBook
                {
                    BookId = 54,
                    Names = new List<string>() { "1 Timothy", "1 Ti", "1Ti" },
                    Language = "en",
                    Chapters = new List<BibleChapter>()
                },
                new BibleBook
                {
                    BookId = 65,
                    Names = new List<string>() { "Jude", "Ju" },
                    Language = "en",
                    Chapters = new List<BibleChapter>()
                },
                new BibleBook
                {
                    BookId = 66,
                    Names = new List<string>() { "Revelation", "Rev", "Re" },
                    Language = "en",
                    Chapters = new List<BibleChapter>()
                }
            };

            // Matthew chapters
            for (int i = 1; i <= 28; i++)
            {
                allBooks[2].Chapters.Add(new BibleChapter { ChapterNumber = i, VerseCount = 30 });
            }

            // 1 Timothy
            for (int i = 1; i <= 4; i++)
            {
                allBooks[3].Chapters.Add(new BibleChapter { ChapterNumber = i, VerseCount = 30 });
            }

            // Jude chapters
            for (int i = 1; i <= 1; i++)
            {
                allBooks[4].Chapters.Add(new BibleChapter { ChapterNumber = i, VerseCount = 30 });
            }

            return allBooks.Where(b => b.Language == _languageProvider.Language).ToList();
        }

    }
}
