namespace RBP.Services.Models
{
    public class Statement
    {
        public Guid Id { get; set; }
        public StatementType Type { get; set; }
        public DateTime Date { get; set; }
        public Guid ProductId { get; set; }
        public Product Product { get; set; }
        public int Weight { get; set; }
        public Guid ResponsibleId { get; set; }
        public Account Responsible { get; set; }
        public int SegmentId { get; set; }
        public WorkshopSegment Segment { get; set; }
        public IList<StatementDefect> Defects { get; set; }
        public string? Comment { get; set; }

        public Statement()
        {
            Defects = new List<StatementDefect>();
        }
    }
}