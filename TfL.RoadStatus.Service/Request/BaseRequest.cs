using System.Runtime.Serialization;

namespace TfL.RoadStatus.Service.Contract
{
    [DataContract]
    public abstract class BaseRequest
    {
        [DataMember]
        public string AppId { get; set; }

        [DataMember]
        public string AppKey { get; set; }

    }
}
