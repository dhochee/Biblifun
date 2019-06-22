using System.Threading.Tasks;

namespace Biblifun.Website.Managers
{
    public interface IScriptureLookupManager
    {
        Task<string> GetVerseHtmlFromSetCode(string verseCode, string language);
    }
}