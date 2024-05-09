namespace RBP.Services.Dto
{
    public class EmployeeUpdateDto
    {
        public Guid Id { get; set; }
        public string Phone { get; set; }
        public string Name { get; set; }
        public string? Comment { get; set; }
        public bool IsActive { get; set; }
        public int SegmentId { get; set; }
        public string Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime EmploymentDate { get; set; }
    }
}