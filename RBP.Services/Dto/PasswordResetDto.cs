namespace RBP.Services.Dto
{
    public class PasswordResetDto
    {
        public Guid AccountId { get; set; }
        public string NewPassword { get; set; }
        public string? OldPassword { get; set; }
    }
}