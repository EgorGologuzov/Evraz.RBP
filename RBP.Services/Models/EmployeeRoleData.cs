namespace RBP.Services.Models
{
    public class EmployeeRoleData
    {
        public static readonly List<string> Genders = new() { "М", "Ж" };

        public int SegmentId { get; set; }
        public string Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime EmploymentDate { get; set; }
    }
}