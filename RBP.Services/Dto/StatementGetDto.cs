namespace RBP.Services.Dto
{
    public class StatementGetDto
    {
        public int? SegmentId { get; set; }
        public Guid? EmployeeId { get; set; }
        public DateTime Date { get; set; }
    }
}