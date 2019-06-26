namespace Biblifun.Data.Repositories.Interfaces
{
    public interface IVerseCacheRepository
    {
        string GetVerseHtmlByCode(string verseCode, string language);

        int? GetSearchCountByCode(string verseCode);

        void AddVerseHtml(string verseCode, string language, string html, int searchCount);
    }
}