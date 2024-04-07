namespace RBP.Web.Dto
{
    public class AdminUpdateDto
    {
        public Guid Id { get; set; }
        public string Phone { get; set; }
        public string Name { get; set; }
        public string Comment { get; set; }
        public bool IsActive { get; set; }
        public string JobTitle { get; set; }
    }
}