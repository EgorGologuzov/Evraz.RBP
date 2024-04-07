using RBP.Web.Models;

namespace RBP.Web.Services.Interfaces
{
    public interface IApiService
    {
        IApiClient Client { get; set; }
    }
}