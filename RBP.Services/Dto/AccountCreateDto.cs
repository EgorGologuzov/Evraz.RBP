namespace RBP.Services.Dto
{
    public class AccountCreateDto
    {
        public string Phone { get; set; }
        public string Name { get; set; }
        public string Role { get; set; }
        public string RoleDataJson { get; set; }
        public string? Comment { get; set; }
    }
}