using System.Runtime.Serialization;

namespace TfL.RoadStatus.Service.Contract
{
    [DataContract]
    public class RoadStatusResponse
    {
        [DataMember]
        public RoadStatusDto RoadStatus { get; set; }
    }
}
