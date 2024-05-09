namespace RBP.Services.Dto
{
    public class AccountReturnDto
    {
        public Guid Id { get; set; }
        public string Phone { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string RoleDataJson { get; set; }
        public DateTime CreationTime { get; set; }
        public string? Comment { get; set; }
        public bool IsActive { get; set; }
    }
}