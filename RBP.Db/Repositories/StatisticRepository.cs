using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RBP.Services.Contracts;
using RBP.Services.Interfaces;
using RBP.Services.Models;
using RBP.Services.Utils;

namespace RBP.Db.Repositories
{
    public class StatisticRepository : IStatisticRepository
    {
        private static readonly MemoryCash<StatisticData> _workshopCash = new();
        private static readonly MemoryCash<StatisticData> _segmentCash = new();

        protected readonly PostgresContext Context;
        protected readonly ILogger Logger;

        public StatisticRepository(PostgresContext context, ILogger<StatisticRepository> logger)
        {
            Context = context;
            Logger = logger;
        }

        private async Task<IEnumerable<StringIntPare>> ProductGroupedWeight(
            StatementType? type = null,
            int? segmentId = null,
            DateTime? start = null,
            DateTime? end = null)
        {
            return await Context.Statements
                .Where(s => (type == null || s.Type == type)
                    && (segmentId == null || s.SegmentId == segmentId)
                    && (start == null || s.Date >= start)
                    && (end == null || s.Date <= end))
                .Include(s => s.Product)
                .GroupBy(s => s.Product.Name)
                .Select(g => new StringIntPare(g.Key, g.Sum(s => s.Weight)))
                .ToListAsync();
        }

        private async Task<IEnumerable<DateIntPare>> DateGroupedWeight(
            StatementType? type = null,
            int? segmentId = null,
            DateTime? start = null,
            DateTime? end = null)
        {
            return await Context.Statements
                .Where(s => (type == null || s.Type == type)
                    && (segmentId == null || s.SegmentId == segmentId)
                    && (start == null || s.Date >= start)
                    && (end == null || s.Date <= end))
                .GroupBy(s => s.Date.Date)
                .Select(g => new DateIntPare(g.Key, g.Sum(s => s.Weight)))
                .ToListAsync();
        }

        private async Task<IEnumerable<StringIntPare>> ProductWeightInStoragePer(int? segmentId, DateTime date)
        {
            return await Context.Statements
                .Where(s => (s.Type == StatementType.Storage || s.Type == StatementType.Transfer)
                    && (segmentId == null || s.SegmentId == segmentId)
                    && s.Date <= date)
                .Include(s => s.Product)
                .GroupBy(s => s.Product.Name)
                .Select(g => new StringIntPare(g.Key, g.Sum(s => s.Type == StatementType.Storage ? s.Weight : -s.Weight)))
                .ToListAsync();
        }

        private async Task<IEnumerable<StringIntPare>> DefectedProductsInStoragePer(int? segmentId, DateTime date)
        {
            return await Context.Statements
                .Include(s => s.Defects)
                .Include(s => s.Product)
                .Where(s => (s.Type == StatementType.Storage || s.Type == StatementType.Transfer)
                    && (segmentId == null || s.SegmentId == segmentId)
                    && s.Date <= date
                    && (s.Defects.Count != 0))
                .GroupBy(s => s.Product.Name)
                .Select(g => new StringIntPare(g.Key, g.Sum(s => s.Type == StatementType.Storage ? s.Weight : -s.Weight)))
                .ToListAsync();
        }

        private async Task<IEnumerable<DateIntPare>> WeightInStorageDynamic(int? segmentId, DateTime start, DateTime end)
        {
            int startWeight = (await ProductWeightInStoragePer(segmentId, start)).Sum(p => p.Value);

            return await Context.Statements
                .Where(s => (s.Type == StatementType.Storage || s.Type == StatementType.Transfer)
                    && (segmentId == null || s.SegmentId == segmentId)
                    && (s.Date >= start)
                    && (s.Date <= end))
                .GroupBy(s => s.Date.Date)
                .Select(g => new DateIntPare(g.Key, g.Sum(s => s.Type == StatementType.Storage ? s.Weight : -s.Weight) + startWeight))
                .ToListAsync();
        }

        public async Task<StatisticData> GetSegmentStatistic(int segmentId, DateTime start, DateTime end)
        {
            start.CheckNotGreater(end, nameof(start));

            DateTime max = await Context.Statements.Where(s => s.SegmentId == segmentId).MaxAsync(s => s.Date);
            end = LogicUtils.Min(end, max);
            start = start.StripSeconds();
            end = end.StripSeconds();
            string key = $"{start}_{end}_{segmentId}";
            StatisticData? result = _segmentCash.Get(key);

            if (result is not null)
            {
                return result;
            }

            result = new StatisticData
            {
                SegmentId = segmentId,
                AcceptedWeightDynamic = await DateGroupedWeight(StatementType.Accept, segmentId, start, end),
                ShippedWeightDynamic = await DateGroupedWeight(StatementType.Shipment, segmentId, start, end),
                AcceptedProductsForPeriod = await ProductGroupedWeight(StatementType.Accept, segmentId, start, end),
                ShippedProductsForPeriod = await ProductGroupedWeight(StatementType.Shipment, segmentId, start, end),
                ProductsInStorageNow = await ProductWeightInStoragePer(segmentId, end),
                DefectedProductsInStorageNow = await DefectedProductsInStoragePer(segmentId, end),
                WeightInStorageDynamic = await WeightInStorageDynamic(segmentId, start, end)
            };

            _segmentCash.Set(key, result);

            return result;
        }

        public async Task<StatisticData> GetAllWorkshopStatistic(DateTime start, DateTime end)
        {
            start.CheckNotGreater(end, nameof(start));

            DateTime max = await Context.Statements.MaxAsync(s => s.Date);
            end = LogicUtils.Min(end, max);
            start = start.StripSeconds();
            end = end.StripSeconds();
            string key = $"{start}_{end}";
            StatisticData? result = _workshopCash.Get(key);

            if (result is not null)
            {
                return result;
            }

            result = new StatisticData
            {
                AcceptedWeightDynamic = await DateGroupedWeight(StatementType.Accept, null, start, end),
                ShippedWeightDynamic = await DateGroupedWeight(StatementType.Shipment, null, start, end),
                AcceptedProductsForPeriod = await ProductGroupedWeight(StatementType.Accept, null, start, end),
                ShippedProductsForPeriod = await ProductGroupedWeight(StatementType.Shipment, null, start, end),
                ProductsInStorageNow = await ProductWeightInStoragePer(null, end),
                DefectedProductsInStorageNow = await DefectedProductsInStoragePer(null, end),
                WeightInStorageDynamic = await WeightInStorageDynamic(null, start, end)
            };

            _workshopCash.Set(key, result);

            return result;
        }
    }
}