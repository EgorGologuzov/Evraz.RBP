namespace RBP.Services.Models
{
    public class SegmentStatistic
    {
        public int SegmentId { get; set; }
        public IEnumerable<KeyValuePair<string, decimal>> ProductsInStorageNow { get; set; }
        public IEnumerable<KeyValuePair<DateTime, decimal>> WeightInStorageDynamic { get; set; }
        public IEnumerable<KeyValuePair<DateTime, decimal>> AcceptedWeightDynamic { get; set; }
        public IEnumerable<KeyValuePair<DateTime, decimal>> ShippedWeightDynamic { get; set; }
        public IEnumerable<KeyValuePair<string, decimal>> AcceptedProductsForPeriod { get; set; }
        public IEnumerable<KeyValuePair<string, decimal>> ShippedProductsForPeriod { get; set; }
        public IEnumerable<KeyValuePair<string, decimal>> DefectedProductsInStorageNow { get; set; }

        public SegmentStatistic()
        {
            ProductsInStorageNow = new List<KeyValuePair<string, decimal>>();
            WeightInStorageDynamic = new List<KeyValuePair<DateTime, decimal>>();
            AcceptedWeightDynamic = new List<KeyValuePair<DateTime, decimal>>();
            ShippedWeightDynamic = new List<KeyValuePair<DateTime, decimal>>();
            AcceptedProductsForPeriod = new List<KeyValuePair<string, decimal>>();
            ShippedProductsForPeriod = new List<KeyValuePair<string, decimal>>();
            DefectedProductsInStorageNow = new List<KeyValuePair<string, decimal>>();
        }
    }
}