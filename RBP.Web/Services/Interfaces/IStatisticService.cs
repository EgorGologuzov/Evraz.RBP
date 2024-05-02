using RBP.Services.Models;

namespace RBP.Web.Services.Interfaces
{
    public interface IStatisticService : IApiService
    {
        Task<SegmentStatistic> GetSegmentStatistic(int segmentId, DateTime start, DateTime end);
        Task<SegmentStatistic> GetAllWorkshopStatistic(DateTime start, DateTime end);
    }
}