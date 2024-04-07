using RBP.Web.Models;
using RBP.Web.Services.Interfaces;

namespace RBP.Web.Services
{
    public class StatisticService : ApiServiceBase, IStatisticService
    {
        public static readonly List<SegmentStatisticData> Statistic = new()
        {
            new()
            {
                SegmentId = 1,
                ProductsInStorageNow = new List<KeyValuePair<string, decimal>>
                {
                    new("Продукт 1", 100),
                    new("Продукт 2", 200),
                    new("Продукт 3", 150),
                    new("Продукт 4", 40),
                    new("Продукт 5", 210),
                },
                WeightInStorageDynamic = new List<KeyValuePair<DateTime, decimal>>
                {
                    new(DateTime.Now, 180),
                    new(DateTime.Now + TimeSpan.FromDays(1), 200),
                    new(DateTime.Now + TimeSpan.FromDays(2), 250),
                    new(DateTime.Now + TimeSpan.FromDays(3), 220),
                    new(DateTime.Now + TimeSpan.FromDays(4), 180),
                    new(DateTime.Now + TimeSpan.FromDays(5), 210),
                    new(DateTime.Now + TimeSpan.FromDays(6), 270),
                },
                AcceptedWeightDynamic = new List<KeyValuePair<DateTime, decimal>>
                {
                    new(DateTime.Now, 35),
                    new(DateTime.Now + TimeSpan.FromDays(1), 29),
                    new(DateTime.Now + TimeSpan.FromDays(2), 23),
                    new(DateTime.Now + TimeSpan.FromDays(3), 30),
                    new(DateTime.Now + TimeSpan.FromDays(4), 32),
                    new(DateTime.Now + TimeSpan.FromDays(5), 23),
                    new(DateTime.Now + TimeSpan.FromDays(6), 23),
                },
                ShippedWeightDynamic = new List<KeyValuePair<DateTime, decimal>>
                {
                    new(DateTime.Now, 30),
                    new(DateTime.Now + TimeSpan.FromDays(1), 25),
                    new(DateTime.Now + TimeSpan.FromDays(2), 29),
                    new(DateTime.Now + TimeSpan.FromDays(3), 35),
                    new(DateTime.Now + TimeSpan.FromDays(4), 32),
                    new(DateTime.Now + TimeSpan.FromDays(5), 33),
                    new(DateTime.Now + TimeSpan.FromDays(6), 20),
                },
                AcceptedProductsForPeriod = new List<KeyValuePair<string, decimal>>
                {
                    new("Продукт 1", 300),
                    new("Продукт 2", 400),
                    new("Продукт 3", 550),
                    new("Продукт 4", 300),
                    new("Продукт 5", 210),
                },
                ShippedProductsForPeriod = new List<KeyValuePair<string, decimal>>
                {
                    new("Продукт 1", 200),
                    new("Продукт 2", 340),
                    new("Продукт 3", 350),
                    new("Продукт 4", 230),
                    new("Продукт 5", 260),
                },
                DefectedProductsInStorageNow = new List<KeyValuePair<string, decimal>>
                {
                    new("Продукт 1", 7),
                    new("Продукт 2", 3),
                    new("Продукт 3", 0),
                    new("Продукт 4", 2),
                    new("Продукт 5", 1),
                },
            }
        };

        public async Task<SegmentStatisticData> GetSegmentStatistic(int segmentId, DateTime start, DateTime end)
        {
            return Statistic.Find(s => s.SegmentId == segmentId) ?? Statistic[0];
        }

        public async Task<SegmentStatisticData> GetAllWorkshopStatistic(DateTime start, DateTime end)
        {
            return Statistic[0];
        }
    }
}