﻿using Microsoft.EntityFrameworkCore;
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

        private async Task<IList<StringIntPare>> ProductGroupedWeight(
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

        private async Task<IList<DateIntPare>> DateGroupedWeight(
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

        private async Task<IList<StringIntPare>> ProductWeightInStoragePer(int? segmentId, DateTime date)
        {
            date = date.Date.AddDays(1);

            return await Context.Statements
                .Where(s => (s.Type == StatementType.Storage || s.Type == StatementType.Transfer)
                    && (segmentId == null || s.SegmentId == segmentId)
                    && s.Date < date)
                .Include(s => s.Product)
                .GroupBy(s => s.Product.Name)
                .Select(g => new StringIntPare(g.Key, g.Sum(s => s.Type == StatementType.Storage ? s.Weight : -s.Weight)))
                .ToListAsync();
        }

        private async Task<IList<StringIntPare>> DefectedProductsInStoragePer(int? segmentId, DateTime date)
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

        private async Task<IList<DateIntPare>> WeightInStorageDynamic(int? segmentId, DateTime start, DateTime end)
        {
            start = start.Date;
            end = end.Date;
            DateTime startNextDay = start.AddDays(1);

            int startWeight = (await ProductWeightInStoragePer(segmentId, start)).Sum(p => p.Value);

            List<DateIntPare> result = await Context.Statements
                .Where(s => (s.Type == StatementType.Storage || s.Type == StatementType.Transfer)
                    && (segmentId == null || s.SegmentId == segmentId)
                    && (s.Date >= startNextDay)
                    && (s.Date < end))
                .GroupBy(s => s.Date.Date)
                .Select(g => new DateIntPare(g.Key, g.Sum(s => s.Type == StatementType.Storage ? s.Weight : -s.Weight) + startWeight))
                .ToListAsync();

            result.Add(new DateIntPare(start, startWeight));

            return result;
        }

        public async Task<StatisticData> GetSegmentStatistic(int segmentId, DateTime start, DateTime end)
        {
            start.CheckNotGreater(end, nameof(start));

            start = start.Date;
            end = end.Date.AddDays(1);

            return new StatisticData
            {
                SegmentId = segmentId,
                AcceptedWeightDynamic = LogicUtils.FillFullListZeroValues(await DateGroupedWeight(StatementType.Accept, segmentId, start, end), start, end),
                ShippedWeightDynamic = LogicUtils.FillFullListZeroValues(await DateGroupedWeight(StatementType.Shipment, segmentId, start, end), start, end),
                AcceptedProductsForPeriod = await ProductGroupedWeight(StatementType.Accept, segmentId, start, end),
                ShippedProductsForPeriod = await ProductGroupedWeight(StatementType.Shipment, segmentId, start, end),
                ProductsInStorageNow = await ProductWeightInStoragePer(segmentId, end),
                DefectedProductsInStorageNow = await DefectedProductsInStoragePer(segmentId, end),
                WeightInStorageDynamic = LogicUtils.FillFullListPreviosValues(await WeightInStorageDynamic(segmentId, start, end), start, end)
            };
        }

        public async Task<StatisticData> GetAllWorkshopStatistic(DateTime start, DateTime end)
        {
            start.CheckNotGreater(end, nameof(start));

            start = start.Date;
            end = end.Date.AddDays(1);

            return new StatisticData
            {
                AcceptedWeightDynamic = LogicUtils.FillFullListZeroValues(await DateGroupedWeight(StatementType.Accept, null, start, end), start, end),
                ShippedWeightDynamic = LogicUtils.FillFullListZeroValues(await DateGroupedWeight(StatementType.Shipment, null, start, end), start, end),
                AcceptedProductsForPeriod = await ProductGroupedWeight(StatementType.Accept, null, start, end),
                ShippedProductsForPeriod = await ProductGroupedWeight(StatementType.Shipment, null, start, end),
                ProductsInStorageNow = await ProductWeightInStoragePer(null, end),
                DefectedProductsInStorageNow = await DefectedProductsInStoragePer(null, end),
                WeightInStorageDynamic = LogicUtils.FillFullListPreviosValues(await WeightInStorageDynamic(null, start, end), start, end)
            };
        }
    }
}