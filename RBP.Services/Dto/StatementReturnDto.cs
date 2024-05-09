using RBP.Services.Models;

namespace RBP.Services.Dto
{
    public class StatementReturnDto
    {
        public Guid Id { get; set; }
        public StatementType Type { get; set; }
        public DateTime Date { get; set; }
        public Guid ProductId { get; set; }
        public int Weight { get; set; }
        public Guid ResponsibleId { get; set; }
        public int SegmentId { get; set; }
        public IList<StatementDefectReturnDto> Defects { get; set; }
        public string? Comment { get; set; }
    }
}