﻿using Biblifun.Data.Models;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Biblifun.Data
{
    public enum VerseParseResult
    {
        Success,
        InvalidSyntax,
        InvalidVerse
    }

    /// <summary>
    /// Provides methods for coverting language-specific verse identifier strings 
    /// into language-agnostic verse metadata and vice versa. Uses the language of
    /// the underlying BibleBookProvider when not specified for any input.
    /// </summary>
    public class VerseParser : IVerseParser
    {
        readonly IBibleBookProvider _bibleBookProvider;

        public VerseParser(IBibleBookProvider bibleBookProvider)
        {
            _bibleBookProvider = bibleBookProvider;
        }

        /// <summary>
        /// 
        /// </summary>
        public VerseParseResult TryParseVerseString(string scriptureCitation, string language, out VerseSetDescriptor verseSet)
        {
            VerseParseResult result;

            verseSet = null;

            scriptureCitation = scriptureCitation.ToLowerInvariant().Trim();

            if (TryParseBook(scriptureCitation, language, out BibleBook book, out string chapterVerse, out result))
            {
                if (TryParseChapter(chapterVerse, book, out BibleChapter chapter, out string verseString, out result))
                {
                    if (TryParseVerse(verseString, chapter, out int startVerse, out int endVerse, out result))
                    {
                        verseSet = new VerseSetDescriptor
                        {
                            BookId = book.BibleBookId,
                            Chapter = chapter.ChapterNumber,
                            IsSingleChapterBook = book.IsSingleChapter,
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
                                  string language,
                                  out BibleBook book,
                                  out string chapterVerse,
                                  out VerseParseResult result)
        {
            //TODO: Check for language validity so we don't end up with a vague null exception
            book = null;
            chapterVerse = null;
            result = VerseParseResult.InvalidVerse;

            // attempt to extract the book from the start of the string
            foreach (var bibleBook in _bibleBookProvider.BibleBooks)
            {
                foreach (var name in bibleBook.GetAllIdentifiersForLanguage(language))
                {
                    if (scriptureString.StartsWith(name.ToLowerInvariant() + " "))
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
                chapter = book.Chapters.FirstOrDefault();
                verseString = chapterVerse;
                result = VerseParseResult.Success;
            }
            else
            {
                var chapterStr = Regex.Match(chapterVerse, @"\d+:").Value;

                if (!string.IsNullOrEmpty(chapterStr))
                {
                    var chapterNum = int.Parse(chapterStr.Replace(":", "").Trim());

                    if (chapterNum > 0 && chapterNum <= book.Chapters.Count)
                    {
                        chapter = book.SortedChapters[chapterNum - 1];
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
                    if (startVerse > chapter.TotalVerses)
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
                                if (tempEndVerse > chapter.TotalVerses)
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

        /// <summary>
        /// Return the language-specific verse display text for the verse or 
        /// sequence identified by the verse set descriptor.
        /// </summary>
        public string GetVerseCitationText(VerseSetDescriptor verseSet, string language)
        {
            var book = _bibleBookProvider.GetBookById(verseSet.BookId);

            var chapterString = book.IsSingleChapter ? "" : $"{verseSet.Chapter}:";

            var verseString = verseSet.Start == verseSet.End ? verseSet.Start.ToString() :
                              verseSet.End == (verseSet.Start + 1) ? $"{verseSet.Start},{verseSet.End}" :
                              $"{verseSet.Start}-{verseSet.End}";

            var bookName = book.GetNameForLanguage(language);

            return $"{bookName} {chapterString}{verseString}";
        }

        /// <summary>
        /// Return the language-specific verse display text for the verse or 
        /// sequence identified by the verse set code.
        /// </summary>
        public string GetVerseCitationText(string verseSetCode, string language)
        {
            var setDescriptor = VerseSetDescriptor.FromCode(verseSetCode);

            return GetVerseCitationText(setDescriptor, language);
        }

        /// <summary>
        /// 
        /// </summary>
        public string GetBookNameById(int bookId, string language)
        {
            return _bibleBookProvider.GetBookById(bookId).GetNameForLanguage(language);
        }
    }
}
