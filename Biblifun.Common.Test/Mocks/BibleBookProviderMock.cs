using Moq;
using System.Collections.Generic;
using System.Linq;

namespace Biblifun.Common.Test.Mocks
{
    public class BibleBookProviderMock : Mock<IBibleBookProvider>
    {
        private const int BOOK_ID_GENESIS = 1;
        private const int BOOK_ID_EXODUS = 2;
        private const int BOOK_ID_MATTHEW = 40;
        private const int BOOK_ID_1TIMOTHY = 54;
        private const int BOOK_ID_JUDE = 65;
        private const int BOOK_ID_REVELATION = 66;

        private Dictionary<string, Dictionary<int, BibleBook>> _booksByLangAndId;

        public BibleBookProviderMock()
        {
            SetupAllBooks();

            this.Setup(b => b.BibleBooks).Returns(_booksByLangAndId["en"].Values.ToList());

            this.Setup(b => b.GetBookById(It.IsAny<int>()))
                .Returns((int bookId) => _booksByLangAndId["en"][bookId]);
        }

        private void SetupAllBooks()
        {
            _booksByLangAndId = new Dictionary<string, Dictionary<int, BibleBook>>();

            var englishBooks = new Dictionary<int, BibleBook>();

            _booksByLangAndId["en"] = englishBooks;

            englishBooks.Add(BOOK_ID_GENESIS, new BibleBook
            {
                BookId = BOOK_ID_GENESIS,
                Name = "Genesis",
                AlternativeNames = new List<string>() { "Gen", "Ge" },
                Language = "en",
                Chapters = new List<BibleChapter>()
            });

            englishBooks.Add(BOOK_ID_EXODUS, new BibleBook
            {
                BookId = BOOK_ID_EXODUS,
                Name = "Exodus",
                AlternativeNames = new List<string>() { "Exo", "Ex" },
                Language = "en",
                Chapters = new List<BibleChapter>()
            });

            englishBooks.Add(BOOK_ID_MATTHEW, new BibleBook
            {
                BookId = BOOK_ID_MATTHEW,
                Name = "Matthew",
                AlternativeNames = new List<string>() { "Matt", "Mat", "Mt" },
                Language = "en",
                Chapters = new List<BibleChapter>()
            });

            englishBooks.Add(BOOK_ID_1TIMOTHY, new BibleBook
            {
                BookId = BOOK_ID_1TIMOTHY,
                Name = "1 Timothy",
                AlternativeNames = new List<string>() { "1 Tim", "1Tim", "1 Ti", "1Ti" },
                Language = "en",
                Chapters = new List<BibleChapter>()
            });
            englishBooks.Add(BOOK_ID_JUDE, new BibleBook
            {
                BookId = BOOK_ID_JUDE,
                Name = "Jude",
                AlternativeNames = new List<string>(),
                Language = "en",
                Chapters = new List<BibleChapter>()
            });

            englishBooks.Add(BOOK_ID_REVELATION, new BibleBook
            {
                BookId = BOOK_ID_REVELATION,
                Name = "Revelation",
                AlternativeNames = new List<string>() { "Rev", "Re" },
                Language = "en",
                Chapters = new List<BibleChapter>()
            });

            // Matthew chapters
            for (int i = 1; i <= 28; i++)
            {
                englishBooks[BOOK_ID_MATTHEW].Chapters.Add(new BibleChapter { ChapterNumber = i, VerseCount = 30 });
            }

            // 1 Timothy chapters
            for (int i = 1; i <= 4; i++)
            {
                englishBooks[BOOK_ID_1TIMOTHY].Chapters.Add(new BibleChapter { ChapterNumber = i, VerseCount = 30 });
            }

            // Jude chapters
            for (int i = 1; i <= 1; i++)
            {
                englishBooks[BOOK_ID_JUDE].Chapters.Add(new BibleChapter { ChapterNumber = i, VerseCount = 30 });
            }
        }
    }
}
