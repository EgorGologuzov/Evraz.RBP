using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using RBP.Services.Exceptions;
using RBP.Services.Models;
using RBP.Services.Utils;
using RBP.Web.Services.Interfaces;
using RBP.Web.Utils;

namespace RBP.Web.Services
{
    public class ApiServiceBase : IApiService
    {
        private readonly HttpClient _http;
        private bool _isHttpConfigured;

        protected readonly ILogger Logger;
        protected Uri DefaultUri = new(JToken.Parse(File.ReadAllText("appsettings.json")).Value<string>("DefaultApiUrl"));
        protected TimeSpan DefaultTimeout = TimeSpan.FromSeconds(10);

        public IApiClient Client { get; set; }

        public ApiServiceBase(HttpClient client, ILogger logger)
        {
            _http = client;
            Logger = logger;
        }

        protected HttpClient Http
        {
            get
            {
                if (_isHttpConfigured)
                {
                    return _http;
                }

                _http.BaseAddress = DefaultUri;
                _http.Timeout = DefaultTimeout;

                if (Client is null || Client.ApiData is null)
                {
                    _isHttpConfigured = true;
                    return _http;
                }

                _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Client.ApiData.Token);

                if (Client.ApiData.RecommendedRefreshTokenTime < DateTime.UtcNow)
                {
                    RefreshClientToken();
                }

                _isHttpConfigured = true;
                return _http;
            }
        }

        private async void RefreshClientToken()
        {
            try
            {
                Client.ApiData = await TryResult(async () =>
                {
                    HttpResponseMessage response = await _http.PostAsync("Auth/Refresh", null);
                    response.ThrowIfUnsuccess();
                    return await response.FromContent<ApiSecrets>();
                });
            }
            catch (NotOkResponseException)
            {
            }
        }

        protected async Task<T> TryResult<T>(
            Func<Task<T>> action,
            Func<BadRequestReturn, string?>? unseccessHandler = null)
        {
            try
            {
                return await action.Invoke();
            }
            catch (UnseccessResponseException ex)
            {
                string json = await ex.Response.Content.ReadAsStringAsync();
                BadRequestReturn badReturn = await ex.Response.FromContent<BadRequestReturn>();
                string message = unseccessHandler?.Invoke(badReturn) ?? "При выполнении запроса произошла ошибка";
                throw new NotOkResponseException(message);
            }
            catch (OperationCanceledException)
            {
                throw new NotOkResponseException("Превышено время ожидания выполнения запроса");
            }
        }
    }
}