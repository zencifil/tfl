using System.Runtime.Serialization;

namespace TfL.RoadStatus.Service.Contract
{
    [DataContract]
    public class GetRoadStatusRequest : BaseRequest
    {
        [DataMember]
        public string RoadName { get; set; }
    }
}
