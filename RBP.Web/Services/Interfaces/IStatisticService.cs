using RBP.Services.Models;

namespace RBP.Web.Services.Interfaces
{
    public interface IStatisticService : IApiService
    {
        Task<StatisticData> GetSegmentStatistic(int segmentId, DateTime start, DateTime end);
        Task<StatisticData> GetAllWorkshopStatistic(DateTime start, DateTime end);
    }
}