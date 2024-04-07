using RBP.Web.Models;

namespace RBP.Web.Services.Interfaces
{
    public interface IStatisticService : IApiService
    {
        Task<SegmentStatisticData> GetSegmentStatistic(int segmentId, DateTime start, DateTime end);
        Task<SegmentStatisticData> GetAllWorkshopStatistic(DateTime start, DateTime end);
    }
}