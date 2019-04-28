﻿using System;
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

        public async Task<IResponse> GetRoadStatus(GetRoadStatusRequest request)
        {
            if (request is null)
                return new ExceptionResponse
                {
                    Result = new ExceptionDto
                    {
                        // can be enum
                        ExceptionType = "ArgumentNullException",
                        HttpStatus = "BadRequest",
                        HttpStatusCode = 400,
                        Message = "Request cannot be null.",
                        RelativeUri = "/Road/",
                        TimeStampUtc = DateTime.UtcNow.ToLongDateString()
                    }
                };

            if (string.IsNullOrEmpty(request.RoadName))
                return new ExceptionResponse
                {
                    Result = new ExceptionDto
                    {
                        // can be enum
                        ExceptionType = "ArgumentNullException",
                        HttpStatus = "BadRequest",
                        HttpStatusCode = 400,
                        Message = "Road name cannot be null.",
                        RelativeUri = "/Road/",
                        TimeStampUtc = DateTime.UtcNow.ToLongDateString()
                    }
                };

            var proxyResponse = await _httpClient.GetAsync($"{BaseUrl}/{request.RoadName}?app_id={request.AppId}&app_key={request.AppKey}");

            if (proxyResponse.IsSuccessStatusCode)
            {
                var roadStatus = new List<RoadStatusDto>();
                JsonConvert.PopulateObject(await proxyResponse.Content.ReadAsStringAsync(), roadStatus);

                return new RoadStatusResponse { Result = roadStatus[0] };
            }
            else
            {
                var exception = new ExceptionDto();
                JsonConvert.PopulateObject(await proxyResponse.Content.ReadAsStringAsync(), exception);

                return new ExceptionResponse { Result = exception };
            }
        }
    }
}
