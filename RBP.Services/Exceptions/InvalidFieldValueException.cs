using System.Collections;

namespace RBP.Services.Exceptions
{
    public class InvalidFieldValueException : Exception
    {
        public readonly string FieldName;

        public override IDictionary Data => new Dictionary<string, string> { { nameof(FieldName), FieldName } };

        private InvalidFieldValueException() : base()
        {
        }

        private InvalidFieldValueException(string? message) : base(message)
        {
        }

        private InvalidFieldValueException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        public InvalidFieldValueException(string fieldName, string? message) : base(message)
        {
            FieldName = fieldName;
        }
    }
}