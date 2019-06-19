using System.Threading.Tasks;
using Biblifun.Common;

namespace Biblifun.WebLookup
{
    /// <summary>
    /// Used to retrieve NWT scripture text from the Watchtower Online Library website.
    /// </summary>
    public interface ISearchCountRetriever
    {
        /// <summary>
        /// Given a verse set identifying a single verse or sequence of verses from a 
        /// single chapter, asynchronously retrieve the search count from the WOL website.
        /// </summary>
        Task<int?> GetSearchCountAsync(VerseSetDescriptor setDescriptor);

        /// <summary>
        /// Given a verse set descriptor, get the url that will retrieve the 
        /// search count from the WOL site.
        /// </summary>
        string GetSearchCountUrl(VerseSetDescriptor verseSet);
    }
}