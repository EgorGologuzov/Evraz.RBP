using RBP.Services.Dto;
using RBP.Services.Models;

namespace RBP.Web.Models
{
    public class SegmentStatisticViewModel : ClientBasedViewModel
    {
        public SegmentStatistic Statistic { get; set; }
        public IList<HandbookEntityReturnDto> AllSegments { get; set; }
        public DateTime? Start { get; set; }
        public DateTime? End { get; set; }

        public HandbookEntityReturnDto Segment => AllSegments.First(s => s.Id == Statistic.SegmentId);

        public SegmentStatisticViewModel(string pageTitle, AccountReturnDto client, SegmentStatistic statistic, IList<HandbookEntityReturnDto> allSegments, DateTime? start, DateTime? end) : base(pageTitle, client)
        {
            Statistic = statistic;
            AllSegments = allSegments;
            Start = start;
            End = end;
        }
    }
}