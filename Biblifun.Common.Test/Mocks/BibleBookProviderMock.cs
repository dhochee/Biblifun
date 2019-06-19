using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Biblifun.Common.Test.Mocks
{
    public class BibleBookProviderMock : Mock<IBibleBookProvider>
    {
        private const int BOOK_ID_MATTHEW = 40;
        private const int BOOK_ID_1TIMOTHY = 54;
        private const int BOOK_ID_JUDE = 65;

        private const string LANG_ENGLISH = "en";
        private const string LANG_SPANISH = "es";

        private string _language;
        private Dictionary<string, Dictionary<int, BibleBook>> _booksByLangAndId;

        public BibleBookProviderMock(string language = LANG_ENGLISH)
        {
            _language = language;

            SetupAllBooks();

            this.Setup(b => b.BibleBooks).Returns(_booksByLangAndId[_language].Values.ToList());

            this.Setup(b => b.GetBookById(It.IsAny<int>()))
                .Returns((int bookId) => _booksByLangAndId[_language][bookId]);
        }

        private void SetupAllBooks()
        {
            _booksByLangAndId = new Dictionary<string, Dictionary<int, BibleBook>>();

            switch(_language)
            {
                case LANG_ENGLISH:
                    SetupEnglishBooks();
                    break;

                case LANG_SPANISH:
                    SetupSpanishBooks();
                    break;
            }
        }

        private void AddChapters(string language)
        {
            // Matthew chapters
            for (int i = 1; i <= 28; i++)
            {
                _booksByLangAndId[language][BOOK_ID_MATTHEW].Chapters.Add(new BibleChapter { ChapterNumber = i, VerseCount = 30 });
            }

            // 1 Timothy chapters
            for (int i = 1; i <= 4; i++)
            {
                _booksByLangAndId[language][BOOK_ID_1TIMOTHY].Chapters.Add(new BibleChapter { ChapterNumber = i, VerseCount = 30 });
            }

            // Jude chapters
            for (int i = 1; i <= 1; i++)
            {
                _booksByLangAndId[language][BOOK_ID_JUDE].Chapters.Add(new BibleChapter { ChapterNumber = i, VerseCount = 30 });
            }
        }

        private void SetupEnglishBooks()
        {
            var englishBooks = new Dictionary<int, BibleBook>();

            englishBooks.Add(BOOK_ID_MATTHEW, new BibleBook
            {
                BookId = BOOK_ID_MATTHEW,
                Name = "Matthew",
                Abbreviations = new List<string>() { "Matt", "Mat", "Mt" },
                Language = LANG_ENGLISH,
                Chapters = new List<BibleChapter>()
            });

            englishBooks.Add(BOOK_ID_1TIMOTHY, new BibleBook
            {
                BookId = BOOK_ID_1TIMOTHY,
                Name = "1 Timothy",
                Abbreviations = new List<string>() { "1 Tim", "1Tim", "1 Ti", "1Ti" },
                Language = LANG_ENGLISH,
                Chapters = new List<BibleChapter>()
            });
            englishBooks.Add(BOOK_ID_JUDE, new BibleBook
            {
                BookId = BOOK_ID_JUDE,
                Name = "Jude",
                Abbreviations = new List<string>(),
                Language = LANG_ENGLISH,
                Chapters = new List<BibleChapter>()
            });

            _booksByLangAndId[LANG_ENGLISH] = englishBooks;

            AddChapters(LANG_ENGLISH);
        }

        private void SetupSpanishBooks()
        {
            var spanishBooks = new Dictionary<int, BibleBook>();

            spanishBooks.Add(BOOK_ID_MATTHEW, new BibleBook
            {
                BookId = BOOK_ID_MATTHEW,
                Name = "Mateo",
                Abbreviations = new List<string>() { "Mat", "Mt" },
                Language = LANG_SPANISH,
                Chapters = new List<BibleChapter>()
            });

            spanishBooks.Add(BOOK_ID_1TIMOTHY, new BibleBook
            {
                BookId = BOOK_ID_1TIMOTHY,
                Name = "1 Timoteo",
                Abbreviations = new List<string>() { "1 Tim", "1Tim", "1 Ti", "1Ti" },
                Language = LANG_SPANISH,
                Chapters = new List<BibleChapter>()
            });
            spanishBooks.Add(BOOK_ID_JUDE, new BibleBook
            {
                BookId = BOOK_ID_JUDE,
                Name = "Judas",
                Abbreviations = new List<string>() { "Jud" },
                Language = LANG_SPANISH,
                Chapters = new List<BibleChapter>()
            });

            _booksByLangAndId[LANG_SPANISH] = spanishBooks;

            AddChapters(LANG_SPANISH);
        }
    }
}
