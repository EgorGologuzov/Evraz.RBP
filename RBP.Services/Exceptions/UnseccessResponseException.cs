using RBP.Services.Models;
using RBP.Services.Utils;

namespace RBP.Services.Exceptions
{
    public class UnseccessResponseException : Exception
    {
        public readonly HttpResponseMessage Response;

        private UnseccessResponseException() : base()
        {
        }

        private UnseccessResponseException(string? message) : base(message)
        {
        }

        private UnseccessResponseException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        public UnseccessResponseException(HttpResponseMessage response, string? message) : base(message)
        {
            Response = response;
        }
    }
}