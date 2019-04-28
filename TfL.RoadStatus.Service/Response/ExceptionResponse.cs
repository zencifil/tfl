using System.Runtime.Serialization;

namespace TfL.RoadStatus.Service.Contract
{
    [DataContract]
    public class ExceptionResponse : IResponse
    {
        [DataMember]
        public BaseDto Result { get; set; }
    }
}
