using RBP.Services.Exceptions;

namespace RBP.Services.Utils
{
    public static class HttpExtensions
    {
        public static StringContent ToJsonContent(this object data)
        {
            return new(
                data.ToJson(),
                System.Text.Encoding.UTF8,
                "application/json"
            );
        }

        public static async Task<T> FromContent<T>(this HttpResponseMessage response)
        {
            return (await response.Content.ReadAsStringAsync()).FromJson<T>();
        }

        public static void ThrowIfUnsuccess(this HttpResponseMessage response, string? messege = null)
        {
            if (!response.IsSuccessStatusCode)
            {
                throw new UnseccessResponseException(response, messege);
            }
        }
    }
}