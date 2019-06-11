using Moq;
using System.Collections.Generic;

namespace Biblifun.Common.Test.Mocks
{
    public class BibleBookProviderMock : Mock<IBibleBookProvider>
    {
        private List<BibleBook> _allBooks;

        public BibleBookProviderMock()
        {
            SetupTestData();

            this.Setup(b => b.BibleBooks).Returns(_allBooks);
        }

        private void SetupTestData()
        {
            _allBooks = new List<BibleBook>()
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
                    Names = new List<string>() { "1 Timothy", "1 Tim", "1Tim", "1 Ti", "1Ti" },
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
                _allBooks[2].Chapters.Add(new BibleChapter { ChapterNumber = i, VerseCount = 30 });
            }

            // 1 Timothy
            for (int i = 1; i <= 4; i++)
            {
                _allBooks[3].Chapters.Add(new BibleChapter { ChapterNumber = i, VerseCount = 30 });
            }

            // Jude chapters
            for (int i = 1; i <= 1; i++)
            {
                _allBooks[4].Chapters.Add(new BibleChapter { ChapterNumber = i, VerseCount = 30 });
            }

        }
    }
}
