using RBP.Services.Models;

namespace RBP.Services.Dto
{
    public class StatementCreateDto
    {
        public StatementType Type { get; set; }
        public Guid ProductId { get; set; }
        public int Weight { get; set; }
        public int SegmentId { get; set; }
        public string? Comment { get; set; }
        public IList<StatementDefectReturnDto> Defects  { get; set; }
    }
}