using System.Net;
using System.Threading.Tasks;

namespace Biblifun.WebLookup
{
    public class WebClientAdapter : IWebClient
    {
        WebClient _webClient;

        public WebClientAdapter(WebClient webClient)
        {
            _webClient = webClient;
        }

        public Task<string> DownloadStringTaskAsync(string url)
        {
            return _webClient.DownloadStringTaskAsync(url);
        }
    }
}
