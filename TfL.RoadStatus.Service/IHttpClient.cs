using System.Net.Http;
using System.Threading.Tasks;

namespace TfL.RoadStatus.Service
{
    public interface IHttpClient
    {
        Task<HttpResponseMessage> GetAsync(string requestUrl);
    }
}
