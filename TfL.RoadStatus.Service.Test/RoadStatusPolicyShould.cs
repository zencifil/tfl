using System;
using System.Threading.Tasks;
using NUnit.Framework;
using TfL.RoadStatus.Service.Contract;
using FluentAssertions;
using Moq;
using System.Net.Http;

namespace TfL.RoadStatus.Service.Test
{
    [TestFixture]
    public class RoadStatusPolicyShould
    {
        private RoadStatusPolicy _policy;
        private Mock<IHttpClient> _mockHttpClient;

        [SetUp]
        public void Setup()
        {
            _mockHttpClient = new Mock<IHttpClient>();
            _policy = new RoadStatusPolicy(_mockHttpClient.Object);
        }

        [Test]
        public void ReturnExceptionResponse_WhenRequestIsNullAsync()
        {
            _policy.GetRoadStatus(default(GetRoadStatusRequest)).Should().BeEquivalentTo(new ExceptionResponse
            {
                Result = new ExceptionDto
                {
                    ExceptionType = "ArgumentNullException",
                    HttpStatus = "BadRequest",
                    HttpStatusCode = 400,
                    Message = "Request cannot be null.",
                    RelativeUri = "/Road/",
                    TimeStampUtc = DateTime.UtcNow.ToLongDateString()
                }
            });
        }

        [Test]
        public void ReturnExceptionResponse_WhenRoadNameIsEmpty()
        {
            _policy.GetRoadStatus(new GetRoadStatusRequest { RoadName = string.Empty }).Should().BeEquivalentTo(new ExceptionResponse
            {
                Result = new ExceptionDto
                {
                    ExceptionType = "ArgumentNullException",
                    HttpStatus = "BadRequest",
                    HttpStatusCode = 400,
                    Message = "Request cannot be null.",
                    RelativeUri = "/Road/",
                    TimeStampUtc = DateTime.UtcNow.ToLongDateString()
                }
            });
        }

        [Test]
        public async Task ReturnExceptionResponse_WhenRoadIsNotFound()
        {
            _mockHttpClient.Setup(hc => hc.GetAsync(It.IsAny<string>())).ReturnsAsync(
                new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Content = new StringContent("{\n  \"$type\": \"Tfl.Api.Presentation.Entities.ApiError, Tfl.Api.Presentation.Entities\",\n  \"timestampUtc\": \"2017-11-21T14:37:39.7206118Z\",\n  \"exceptionType\": \"EntityNotFoundException\",\n  \"httpStatusCode\": 404,\n  \"httpStatus\": \"NotFound\",\n  \"relativeUri\": \"/Road/A233\",\n  \"message\": \"The following road id is not recognised: A233\"\n}\n")
                });

            var expectedResult = new ExceptionDto
            {
                ExceptionType = "EntityNotFoundException",
                HttpStatus = "NotFound",
                HttpStatusCode = 404,
                Message = "The following road id is not recognised: A233",
                RelativeUri = "/Road/A233",
                TimeStampUtc = "2017-11-21T14:37:39.7206118Z"
            };

            var response = await _policy.GetRoadStatus(new GetRoadStatusRequest { RoadName = "Test", AppId = "Test", AppKey = "Test" });

            Assert.That(response is ExceptionResponse);
            response.Result.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public async Task ReturnRoadStatusResponse_WhenRoadIsFound()
        {
            _mockHttpClient.Setup(hc => hc.GetAsync(It.IsAny<string>())).ReturnsAsync(
                new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Content = new StringContent("[\n  {\n    \"$type\": \"Tfl.Api.Presentation.Entities.RoadCorridor, Tfl.Api.Presentation.Entities\",\n    \"id\": \"a2\",\n    \"displayName\": \"A2\",\n    \"statusSeverity\": \"Good\",\n    \"statusSeverityDescription\": \"No Exceptional Delays\",\n    \"bounds\": \"[[-0.0857,51.44091],[0.17118,51.49438]]\",\n    \"envelope\": \"[[-0.0857,51.44091],[-0.0857,51.49438],[0.17118,51.49438],[0.17118,51.44091],[-0.0857,51.44091]]\",\n    \"url\": \"/Road/a2\"\n  }\n]\n")
                });

            var expectedResult = new RoadStatusDto
            {
                Bounds = "[[-0.0857,51.44091],[0.17118,51.49438]]",
                DisplayName = "A2",
                Envelope = "[[-0.0857,51.44091],[-0.0857,51.49438],[0.17118,51.49438],[0.17118,51.44091],[-0.0857,51.44091]]",
                Id = "a2",
                StatusSeverity = "Good",
                StatusSeverityDescription = "No Exceptional Delays",
                Url = "/Road/a2"
            };

            var response = await _policy.GetRoadStatus(new GetRoadStatusRequest { RoadName = "Test", AppId = "Test", AppKey = "Test" });

            Assert.That(response is RoadStatusResponse);
            response.Result.Should().BeEquivalentTo(expectedResult);
        }
    }
}
