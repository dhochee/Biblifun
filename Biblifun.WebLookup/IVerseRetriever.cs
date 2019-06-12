using System.Threading.Tasks;
using Biblifun.Common;

namespace Biblifun.WebLookup
{
    /// <summary>
    /// Used to retrieve NWT scripture text from the Watchtower Online Library website.
    /// </summary>
    public interface IVerseRetriever
    {
        /// <summary>
        /// Given a verse set identifying a single verse or sequence of verses from a 
        /// single chapter, asynchronously retrieve the language-specific HTML from the 
        /// WOL website, modifying the returned HTML in the process to suit our use.
        /// </summary>
        Task<string> GetVerseHtmlAsync(IVerseSetId verseSetId);

        /// <summary>
        /// Given a verse set id, get the url that will retrieve the 
        /// verse from the WOL site using the configured language.
        /// </summary>
        string GetVerseUrl(IVerseSetId verseSetId);
    }
}