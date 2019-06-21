using Biblifun.Data.Models;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Biblifun.Data
{
    /// <summary>
    /// This class is a helper utility which has the sole purpose of gathering Bible
    /// book, chapter, and verse metadata from various sources and consolidating into
    /// a single JSON file which may be used for integration tests and for seeding of
    /// the metadata in the database. This includes populating the language-specific
    /// book name metadata required for user lookups.
    /// </summary>
    public class BibleMetadataBuilder : IBibleMetadataBuilder
    {
        private const string METADATA_FOLDER = @".\BibleMetadataJSON\";
        private const string CHAPTER_AND_VERSES_FILE = "ChaptersAndVerses.json";
        private const string JSON_OUTPUT_FILE = "BibleMetaData.json";

        private int[][] _verseCountByChapter;

        
        public void SaveMetadataToFile()
        {
            var metaData = GetBibleMetadata();

            var settings = new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                Formatting = Formatting.Indented
            };

            var metaDataJson = JsonConvert.SerializeObject(metaData, settings);

            File.WriteAllText(Path.Combine(METADATA_FOLDER, JSON_OUTPUT_FILE), metaDataJson);
        }

        public List<BibleBook> LoadMetaDataFromFile()
        {
            var metadataFilePath = Directory.GetFiles(".\\", JSON_OUTPUT_FILE, SearchOption.AllDirectories).FirstOrDefault();

            string metadataJson = File.ReadAllText(metadataFilePath);

            return JsonConvert.DeserializeObject<List<BibleBook>>(metadataJson);
        }

        public List<BibleBook> GetBibleMetadata()
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

            var allBooks = new List<BibleBook>();

            var allBookNames = GetAllBibleBookNames();

            LoadVerseCountByChapter();

            for (int i = 1; i <= allBookNames.Count; i++)
            {
                var name = allBookNames[i - 1];
                var book = RetrieveBibleBookDetails(i, name);

                allBooks.Add(book);
            }

            PopulateBibleNamesByLanguage("en", allBooks);
            PopulateBibleNamesByLanguage("es", allBooks);

            return allBooks;
        }

        public void LoadVerseCountByChapter()
        {
            var fileText = File.ReadAllText(Path.Combine(METADATA_FOLDER, CHAPTER_AND_VERSES_FILE));

            _verseCountByChapter = JsonConvert.DeserializeObject<int[][]>(fileText);
        }

        private List<string> GetAllBibleBookNames()
        {
            var bookNames = new List<string>()
            {
                "Genesis",
                "Exodus",
                "Leviticus",
                "Numbers",
                "Deuteronomy",
                "Joshua",
                "Judges",
                "Ruth",
                "1 Samuel",
                "2 Samuel",
                "1 Kings",
                "2 Kings",
                "1 Chronicles",
                "2 Chronicles",
                "Ezra",
                "Nehemiah",
                "Esther",
                "Job",
                "Psalms",
                "Proverbs",
                "Ecclesiastes",
                "Song of Solomon",
                "Isaiah",
                "Jeremiah",
                "Lamentations",
                "Ezekiel",
                "Daniel",
                "Hosea",
                "Joel",
                "Amos",
                "Obadiah",
                "Jonah",
                "Micah",
                "Nahum",
                "Habakkuk",
                "Zephaniah",
                "Haggai",
                "Zechariah",
                "Malachi",
                "Matthew",
                "Mark",
                "Luke",
                "John",
                "Acts",
                "Romans",
                "1 Corinthians",
                "2 Corinthians",
                "Galatians",
                "Ephesians",
                "Philippians",
                "Colossians",
                "1 Thessalonians",
                "2 Thessalonians",
                "1 Timothy",
                "2 Timothy",
                "Titus",
                "Philemon",
                "Hebrews",
                "James",
                "1 Peter",
                "2 Peter",
                "1 John",
                "2 John",
                "3 John",
                "Jude",
                "Revelation"
            };

            return bookNames;
        }

        public BibleBook RetrieveBibleBookDetails(int bookId, string bookName)
        {
            var book = new BibleBook
            {
                BibleBookId = bookId,
                Name = bookName,
            };

            var chapterCount = _verseCountByChapter[bookId].Length - 1;

            book.TotalChapters = chapterCount;

            for (int i = 1; i <= chapterCount; i++)
            {
                var chapter = RetrieveChapterDetails(bookId, i);

                chapter.Book = book;

                book.Chapters.Add(chapter);
            }

            return book;
        }

        public BibleChapter RetrieveChapterDetails(int bookId, int chapterNumber)
        {
            var chapter = new BibleChapter
            {
                ChapterNumber = chapterNumber,
                BibleBookId = bookId
            };

            chapter.TotalVerses = _verseCountByChapter[bookId][chapterNumber];

            return chapter;
        }

        public void PopulateBibleNamesByLanguage(string language, List<BibleBook> bibleBooks)
        {
            var fileText = File.ReadAllText($@".\BibleMetadataJSON\{language}.json");

            dynamic data = JsonConvert.DeserializeObject(fileText);

            foreach (var book in bibleBooks)
            {
                var bookData = data.editionData.books[book.BibleBookId.ToString()];

                var bookName = bookData.standardName.Value;

                // get abbreviations with and without diacritics
                var altNames = (new List<string>
                    {
                        bookData.standardAbbreviation.Value,
                        bookData.standardAbbreviation.Value.Replace(".", ""),
                        bookData.officialAbbreviation.Value,
                        RemoveDiacritics(bookData.standardAbbreviation.Value),
                        RemoveDiacritics(bookData.standardAbbreviation.Value.Replace(".", "")),
                        RemoveDiacritics(bookData.officialAbbreviation.Value)
                    })
                    .Where(s => !string.IsNullOrEmpty(s))
                    .Distinct()
                    .ToList();

                // make the name, without diacritics, an alternate name
                var strippedName = RemoveDiacritics(bookName);

                if (strippedName != bookName)
                {
                    altNames.Add(strippedName);
                }

                book.BookNames.Add(new BibleBookName
                {
                    BibleBookId = book.BibleBookId,
                    Book = book,
                    Name = bookName,
                    Language = language,
                    AlternateIdentifiers = string.Join(",", altNames)
                });
            }
        }

        public static string RemoveDiacritics(string str)
        {
            return Encoding.ASCII.GetString(Encoding.GetEncoding(1251).GetBytes(str));
        }
    }
}
