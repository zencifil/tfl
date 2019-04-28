using System.Runtime.Serialization;

namespace TfL.RoadStatus.Service.Contract
{
    [DataContract]
    public class ExceptionDto : BaseDto
    {
        [DataMember]
        public string TimeStampUtc { get; set; }

        [DataMember]
        public string ExceptionType { get; set; }

        [DataMember]
        public int HttpStatusCode { get; set; }

        [DataMember]
        public string HttpStatus { get; set; }

        [DataMember]
        public string RelativeUrl { get; set; }

        [DataMember]
        public string Message { get; set; }

    }
}
