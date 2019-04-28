using System.Runtime.Serialization;

namespace TfL.RoadStatus.Service.Contract
{
    [DataContract]
    public class RoadStatusResponse : IResponse
    {
        [DataMember]
        public BaseDto Result { get; set; }
    }
}
