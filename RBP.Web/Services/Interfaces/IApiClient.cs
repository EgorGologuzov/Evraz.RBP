using RBP.Services.Models;

namespace RBP.Web.Services.Interfaces
{
    public interface IApiClient
    {
        ApiSecrets ApiData { get; set; }
    }
}