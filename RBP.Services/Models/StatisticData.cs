namespace RBP.Services.Models
{
    public class StatisticData
    {
        public int? SegmentId { get; set; }
        public IEnumerable<DateIntPare> AcceptedWeightDynamic { get; set; }
        public IEnumerable<DateIntPare> ShippedWeightDynamic { get; set; }
        public IEnumerable<DateIntPare> WeightInStorageDynamic { get; set; }
        public IEnumerable<StringIntPare> AcceptedProductsForPeriod { get; set; }
        public IEnumerable<StringIntPare> DefectedProductsInStorageNow { get; set; }
        public IEnumerable<StringIntPare> ProductsInStorageNow { get; set; }
        public IEnumerable<StringIntPare> ShippedProductsForPeriod { get; set; }

        public StatisticData()
        {
            ProductsInStorageNow = new List<StringIntPare>();
            WeightInStorageDynamic = new List<DateIntPare>();
            AcceptedWeightDynamic = new List<DateIntPare>();
            ShippedWeightDynamic = new List<DateIntPare>();
            AcceptedProductsForPeriod = new List<StringIntPare>();
            ShippedProductsForPeriod = new List<StringIntPare>();
            DefectedProductsInStorageNow = new List<StringIntPare>();
        }
    }
}