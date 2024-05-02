using System.Data;

namespace RBP.Services.Dto
{
    public class StatementCreateDto
    {
        public StatementType Type { get; set; }
        public Guid ProductId { get; set; }
        public decimal Weight { get; set; }
        public int SegmentId { get; set; }
        public string Comment { get; set; }
        public string DefectsJson { get; set; }
    }
}