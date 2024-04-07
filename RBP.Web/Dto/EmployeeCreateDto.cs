namespace RBP.Web.Dto
{
    public class EmployeeCreateDto
    {
        public string Phone { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public int SegmentId { get; set; }
        public string Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime EmploymentDate { get; set; }
    }
}