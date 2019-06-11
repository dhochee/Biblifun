using System.Text.RegularExpressions;

namespace Biblifun.Common
{
    public enum VerseParseResult
    {
        Success,
        InvalidSyntax,
        InvalidVerse
    }


    public class VerseParser
    {
        readonly IBibleBookProvider _bibleBookProvider;

        public VerseParser(IBibleBookProvider bibleBookProvider)
        {
            _bibleBookProvider = bibleBookProvider;
        }

        public VerseParseResult TryParseVerseString(string scriptureString, out VerseSet verseSet)
        {
            VerseParseResult result;

            verseSet = null;

            scriptureString = scriptureString.ToLower().Trim();

            if(TryParseBook(scriptureString, out BibleBook book, out string chapterVerse, out result))
            {
                if(TryParseChapter(chapterVerse, book, out BibleChapter chapter, out string verseString, out result))
                {
                    if(TryParseVerse(verseString, chapter, out int startVerse, out int endVerse, out result))
                    {
                        verseSet = new VerseSet
                        {
                            BookId = book.BookId,
                            Chapter = chapter.ChapterNumber,
                            Start = startVerse,
                            End = endVerse
                        };

                        result = VerseParseResult.Success;
                    }
                }
            }

            return result;
        }

        private bool TryParseBook(string scriptureString, 
                                  out BibleBook book, 
                                  out string chapterVerse, 
                                  out VerseParseResult result)
        {
            book = null;
            chapterVerse = null;
            result = VerseParseResult.InvalidVerse;

            // attempt to extract the book from the start of the string
            foreach (var bibleBook in _bibleBookProvider.GetBibleBooks())
            {
                foreach (var name in bibleBook.Names)
                {
                    if (scriptureString.StartsWith(name.ToLower() + " "))
                    {
                        book = bibleBook;
                        chapterVerse = scriptureString.Substring(name.Length).Trim();
                        result = VerseParseResult.Success;
                        return true;
                    }
                }
            }

            return false;
        }

        private bool TryParseChapter(string chapterVerse, 
                                     BibleBook book, 
                                     out BibleChapter chapter, 
                                     out string verseString, 
                                     out VerseParseResult result)
        {
            chapter = null;
            verseString = null;
            result = VerseParseResult.InvalidSyntax;

            if (book.IsSingleChapter)
            {
                chapter = book.Chapters[0];
                verseString = chapterVerse;
                result = VerseParseResult.Success;
            }
            else
            {
                var chapterStr = Regex.Match(chapterVerse, @"\d+:").Value;

                if (chapterStr != null)
                {
                    var chapterNum = int.Parse(chapterStr.Replace(":", "").Trim());

                    if (chapterNum > 0 && chapterNum <= book.Chapters.Count)
                    {
                        chapter = book.Chapters[chapterNum - 1];
                        verseString = chapterVerse.Substring(chapterVerse.IndexOf(":") + 1).Trim();
                        result = VerseParseResult.Success;
                    }
                    else
                    {
                        result = VerseParseResult.InvalidVerse;
                    }
                }
            }

            return result == VerseParseResult.Success;
        }

        private bool TryParseVerse(string verseString,
                                   BibleChapter chapter,
                                   out int startVerse, 
                                   out int endVerse, 
                                   out VerseParseResult result)
        {
            startVerse = 0;
            endVerse = 0;
            result = VerseParseResult.InvalidSyntax;

            if (!string.IsNullOrWhiteSpace(verseString))
            {
                var startVerseString = Regex.Match(verseString, "\\d+").Value;

                if (int.TryParse(startVerseString, out startVerse))
                {
                    if (startVerse > chapter.VerseCount)
                    {
                        // start verse is greater than the number of verses in the chapter
                        result = VerseParseResult.InvalidVerse; 
                    }
                    else
                    {
                        var endVerseString = Regex.Match(verseString, "[-,]\\d+").Value.Replace("-", "").Replace(",", "");

                        if (!string.IsNullOrEmpty(endVerseString))
                        {
                            if (int.TryParse(endVerseString, out int tempEndVerse))
                            {
                                if (tempEndVerse > chapter.VerseCount)
                                {
                                    // the end verse is greater than the number of verses in the chapter
                                    result = VerseParseResult.InvalidVerse;
                                }
                                else if (tempEndVerse < 1 || (verseString.Contains(",") && tempEndVerse != (startVerse + 1)))
                                {
                                    // end verse is 0 or there were two non-sequential verses separated by a comma
                                    result = VerseParseResult.InvalidSyntax;
                                }
                                else
                                {
                                    endVerse = tempEndVerse;
                                    result = VerseParseResult.Success;
                                }
                            }
                        }
                        else
                        {
                            // no end verse specified, so use the start as the end
                            endVerse = startVerse;
                            result = VerseParseResult.Success;
                        }
                    }
                }
            }

            return result == VerseParseResult.Success;
        }
    }
}
