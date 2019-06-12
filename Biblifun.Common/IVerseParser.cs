namespace Biblifun.Common
{
    public interface IVerseParser
    {
        IVerseSetId GetVerseSetFromId(string verseSetId);

        VerseParseResult TryParseVerseString(string scriptureString, out IVerseSetId verseSet);

        string GetVerseDisplayText(IVerseSetId verseSet);

        string GetVerseDisplayText(string verseSetId);

        string GetBookNameById(int bookId);

        bool IsSingleChapterBook(int bookId);
    }
}