namespace RBP.Services.Models
{
    public class Account
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; }
        public string RoleDataClassName { get; set; }
        public string RoleDataJson { get; set; }
        public DateTime CreateTime { get; set; }
    }
}