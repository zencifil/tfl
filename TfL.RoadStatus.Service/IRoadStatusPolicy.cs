using System.Threading.Tasks;
using TfL.RoadStatus.Service.Contract;

namespace TfL.RoadStatus.Service
{
    public interface IRoadStatusPolicy
    {
        Task<RoadStatusResponse> GetRoadStatus(GetRoadStatusRequest request);
    }
}
