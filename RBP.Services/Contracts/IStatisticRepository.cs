using RBP.Services.Models;

namespace RBP.Services.Contracts
{
    public interface IStatisticRepository
    {
        Task<StatisticData> GetSegmentStatistic(int segmentId, DateTime start, DateTime end);
        Task<StatisticData> GetAllWorkshopStatistic(DateTime start, DateTime end);
    }
}