using System.Threading.Tasks;
using Biblifun.Common;

namespace Biblifun.WebLookup
{
    public interface IVerseRetriever
    {
        Task<string> GetVerseHtmlAsync(IVerseSetId verseSetId);
        string GetVerseUrl(IVerseSetId verseSetId);
    }
}