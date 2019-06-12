using System.Threading.Tasks;

namespace Biblifun.WebLookup
{
    public interface IWebClient
    {
        Task<string> DownloadStringTaskAsync(string url);
    }
}
