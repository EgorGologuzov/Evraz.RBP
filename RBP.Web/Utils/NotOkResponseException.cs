namespace RBP.Web.Utils
{
    public class NotOkResponseException : Exception
    {
        public readonly int Code;

        private NotOkResponseException() : base()
        {
        }

        public NotOkResponseException(string? message) : base(message)
        {
        }

        private NotOkResponseException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        public NotOkResponseException(int code, string? message) : base(message)
        {
            Code = code;
        }
    }
}