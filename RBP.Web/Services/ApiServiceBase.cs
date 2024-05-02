using RBP.Services.Models;
using RBP.Web.Services.Interfaces;

namespace RBP.Web.Services
{
    public class ApiServiceBase : IApiService
    {
        public IApiClient Client { get; set; }

        protected ApiData ApiData
        {
            get
            {
                if (Client is null || Client.ApiData is null)
                {
                    throw new InvalidOperationException("Свойство Client или Client.ApiData = null в ApiServiceBase. Они должны быть заданы перед использованием IApiService.");
                }

                return Client.ApiData;
            }
        }
    }
}