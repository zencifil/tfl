using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;
using TfL.RoadStatus.Service.Contract;

namespace TfL.RoadStatus.Service
{
    public class RoadStatusPolicy : IRoadStatusPolicy
    {
        // to make it simpler
        private const string BaseUrl = "https://api.tfl.gov.uk/Road";
        private readonly IHttpClient _httpClient;

        public RoadStatusPolicy(IHttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<RoadStatusResponse> GetRoadStatus(GetRoadStatusRequest request)
        {
            if (request is null || string.IsNullOrEmpty(request.RoadName))
                return null;

            var proxyResponse = await _httpClient.GetAsync($"{BaseUrl}/{request.RoadName}?app_id={request.AppId}&app_key={request.AppKey}");

            if (!proxyResponse.IsSuccessStatusCode)
                return null;

            var roadStatus = new List<RoadStatusDto>();
            JsonConvert.PopulateObject(await proxyResponse.Content.ReadAsStringAsync(), roadStatus);

            return new RoadStatusResponse { RoadStatus = roadStatus[0] };
        }
    }
}
