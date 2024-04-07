namespace RBP.Web.Utils
{
    public class NotOkResponseException : Exception
    {
        public int Code { get; set; }

        public NotOkResponseException() : base()
        {
        }

        public NotOkResponseException(string? message) : base(message)
        {
        }

        public NotOkResponseException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}