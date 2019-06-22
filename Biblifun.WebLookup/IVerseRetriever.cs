using System.Threading.Tasks;
using Biblifun.Data;

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
        Task<string> GetVerseHtmlAsync(VerseSetDescriptor verseSet, string language);

        /// <summary>
        /// Given a verse set descriptor, get the url that will retrieve the 
        /// verse from the WOL site using the specified language.
        /// </summary>
        string GetVerseUrl(VerseSetDescriptor verseSet, string language);
    }
}