namespace Biblifun.Data
{
    /// <summary>
    /// Provides methods for coverting language-specific verse identifier strings 
    /// into language-agnostic verse metadata and vice versa. 
    /// </summary>
    public interface IVerseParser
    {
        /// <summary>
        /// Given a language-specific citation string, attempt to determine the book, chapter, 
        /// and verses identified by the citation.
        /// </summary>
        /// <param name="scriptureCitation">The input citation string, e.g., "Matthew 28:19,20" or "Mateo 24:14".</param>
        /// <param name="language">Language code identifier, such as "en" for English. 
        /// <param name="verseSet">The resultant output. Null if parsing was not successful.</param>
        /// <returns>A VerseParseResult which indicates if the citation was matched, invalid syntax 
        /// (could not be parsed), or an invalid verse (e.g., specified a non-existent chapter or verse).</returns>
        VerseParseResult TryParseVerseString(string scriptureCitation, string language, out VerseSetDescriptor verseSet);

        /// <summary>
        /// Given a verse set descriptor, return the citation display for the specified language.
        /// </summary>
        /// <param name="verseSet"></param>
        /// <param name="language">Optional language code identifier, such as "en" for English. 
        /// <returns>The language-specific citation, e.g. "1 Timothy 3:1-5"</returns>
        string GetVerseCitationText(VerseSetDescriptor verseSet, string language);

        /// <summary>
        /// Given a verse set code, return the citation display for the specified language.
        /// </summary>
        /// <param name="verseSetCode">String identifying the verse sequence. Example: "40028019020" for Matthew 28:19,20.</param>
        /// <param name="language">Optional language code identifier, such as "en" for English. 
        /// <returns>The language-specific citation, e.g. "1 Timothy 3:1-5"</returns>
        string GetVerseCitationText(string verseSetCode, string language);

        /// <summary>
        /// Helper method to convert a book id into a language-specific book name.
        /// </summary>
        /// <param name="bookId">The Bible book id, such as 1 for Genesis, or 66 for Revelation.</param>
        /// <param name="language">Optional language code identifier, such as "en" for English. 
        string GetBookNameById(int bookId, string language);
    }
}