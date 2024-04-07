namespace RBP.Web.Models
{
    public class SegmentStatisticViewModel : ClientBasedViewModel
    {
        public SegmentStatisticData Statistic { get; set; }
        public IList<HandbookEntityData> AllSegments { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }

        public HandbookEntityData Segment => AllSegments.First(s => s.Id == Statistic.SegmentId);

        public SegmentStatisticViewModel(string pageTitle, AccountData client, SegmentStatisticData statistic, IList<HandbookEntityData> allSegments, DateTime? start, DateTime? end) : base(pageTitle, client)
        {
            Statistic = statistic;
            AllSegments = allSegments;
            Start = start;
            End = end;
        }
    }
}