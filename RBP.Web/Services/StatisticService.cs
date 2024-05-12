using RBP.Services.Dto;
using RBP.Services.Exceptions;
using RBP.Services.Models;
using RBP.Services.Utils;
using RBP.Web.Services.Interfaces;

namespace RBP.Web.Services
{
    public class StatisticService : ApiServiceBase, IStatisticService
    {
        public StatisticService(HttpClient client, ILogger<StatisticService> logger) : base(client, logger)
        {
        }

        public Task<StatisticData> GetSegmentStatistic(int segmentId, DateTime start, DateTime end)
        {
            StatisticGetDto data = new()
            {
                SegmentId = segmentId,
                Start = start.Date,
                End = end.Date.AddDays(1)
            };

            return TryResult(
            action: async () =>
            {
                HttpResponseMessage response = await Http.PostAsync("Statistic/ForSegment", data.ToJsonContent());
                response.ThrowIfUnsuccess();

                return await response.FromContent<StatisticData>();
            },
            unseccessHandler: (data) => data.Exception == nameof(InvalidFieldValueException) ? "Неверно выбран диапазон дат" : null);
        }

        public Task<StatisticData> GetAllWorkshopStatistic(DateTime start, DateTime end)
        {
            StatisticGetDto data = new()
            {
                Start = start.Date,
                End = end.Date.AddDays(1)
            };

            return TryResult(
            action: async () =>
            {
                HttpResponseMessage response = await Http.PostAsync("Statistic/ForWorkshop", data.ToJsonContent());
                response.ThrowIfUnsuccess();

                return await response.FromContent<StatisticData>();
            },
            unseccessHandler: (data) => data.Exception == nameof(InvalidFieldValueException) ? "Неверно выбран диапазон дат" : null);
        }
    }
}