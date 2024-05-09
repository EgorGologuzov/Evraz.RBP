namespace RBP.Services.Dto
{
    public class AccountUpdateDto
    {
        public Guid Id { get; set; }
        public string Phone { get; set; }
        public string Name { get; set; }
        public string RoleDataJson { get; set; }
        public string? Comment { get; set; }
        public bool IsActive { get; set; }
    }
}