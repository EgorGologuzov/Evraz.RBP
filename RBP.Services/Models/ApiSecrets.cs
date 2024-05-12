using RBP.Services.Dto;

namespace RBP.Services.Models
{
    public class ApiSecrets
    {
        public string Token { get; set; }
        public DateTime TokenExpirationTime { get; set; }
        public DateTime RecommendedRefreshTokenTime { get; set; }
        public AccountReturnDto Account { get; set; }
    }
}