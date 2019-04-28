using System.Runtime.Serialization;

namespace TfL.RoadStatus.Service.Contract
{
    [DataContract]
    public class RoadStatusDto : BaseDto
    {
        [DataMember]
        public string Id { get; set; }

        [DataMember]
        public string DisplayName { get; set; }

        [DataMember]
        public string StatusSeverity { get; set; }

        [DataMember]
        public string StatusSeverityDescription { get; set; }

        [DataMember]
        public string Bounds { get; set; }

        [DataMember]
        public string Envelope { get; set; }

        [DataMember]
        public string Url { get; set; }
    }
}
