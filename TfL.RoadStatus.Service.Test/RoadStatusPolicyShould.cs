using System.Net.Http;
using System.Threading.Tasks;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using TfL.RoadStatus.Service.Contract;

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
        public async Task ReturnNull_WhenRequestIsNullAsync()
        {
            var response = await _policy.GetRoadStatus(default(GetRoadStatusRequest));

            Assert.IsNull(response);
        }

        [Test]
        public async Task ReturnNull_WhenRoadNameIsEmpty()
        {
            var response = await _policy.GetRoadStatus(new GetRoadStatusRequest { RoadName = string.Empty });

            Assert.IsNull(response);
        }

        [Test]
        public async Task ReturnNull_WhenRoadIsNotFound()
        {
            _mockHttpClient.Setup(hc => hc.GetAsync(It.IsAny<string>())).ReturnsAsync(
                new HttpResponseMessage
                {
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Content = new StringContent("not found")
                });

            var response = await _policy.GetRoadStatus(new GetRoadStatusRequest { RoadName = "Test", AppId = "Test", AppKey = "Test" });

            Assert.IsNull(response);
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
            response.RoadStatus.Should().BeEquivalentTo(expectedResult);
        }
    }
}
