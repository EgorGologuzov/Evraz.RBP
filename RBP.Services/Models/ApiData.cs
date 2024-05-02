namespace RBP.Services.Models
{
    public class ApiData
    {
        public string Token { get; set; }
        public DateTime TokenExpirationTime { get; set; }
        public DateTime RecommendedRefreshTokenTime { get; set; }
    }
}