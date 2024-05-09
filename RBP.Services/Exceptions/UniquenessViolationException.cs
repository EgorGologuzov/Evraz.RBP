using System.Collections;

namespace RBP.Services.Exceptions
{
    public class UniquenessViolationException : Exception
    {
        public readonly string Entity;
        public readonly string[] Fields;

        public override IDictionary Data => new Dictionary<string, object> { { nameof(Entity), Entity }, { nameof(Fields), Fields } };

        private UniquenessViolationException() : base()
        {
        }

        private UniquenessViolationException(string? message) : base(message)
        {
        }

        private UniquenessViolationException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        public UniquenessViolationException(string? message, string entity, params string[] fields) : base(message)
        {
            Entity = entity;
            Fields = fields;
        }
    }
}