using System;
using System.Threading.Tasks;
using NUnit.Framework;
using TfL.RoadStatus.Service.Contract;
using FluentAssertions;

namespace TfL.RoadStatus.Service.Test
{
    [TestFixture]
    public class RoadStatusPolicyShould
    {
        private RoadStatusPolicy _policy;

        [SetUp]
        public void Setup()
        {
            _policy = new RoadStatusPolicy();
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
                    RelativeUrl = "/Road/",
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
                    RelativeUrl = "/Road/",
                    TimeStampUtc = DateTime.UtcNow.ToLongDateString()
                }
            });
        }


    }
}
