using System.Net.Http;
using System.Threading.Tasks;

namespace TfL.RoadStatus.Service
{
    public class HttpClient : IHttpClient
    {

        public async Task<HttpResponseMessage> GetAsync(string requestUrl)
        {
            return await new System.Net.Http.HttpClient().GetAsync(requestUrl);
        }
    }
}
