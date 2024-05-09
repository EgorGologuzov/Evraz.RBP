using System.Collections;

namespace RBP.Services.Exceptions
{
    public class EntityNotExistsException : Exception
    {
        private readonly string _message;

        public readonly object Id;

        public override IDictionary Data => new Dictionary<string, object> { { nameof(Id), Id } };
        public override string Message => _message;

        private EntityNotExistsException() : base()
        {
        }

        private EntityNotExistsException(string? message) : base(message)
        {
        }

        private EntityNotExistsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }

        public EntityNotExistsException(object id, string? message = null) : base(message)
        {
            Id = id;
            _message = message ?? "Сущность с таким идентификатором не найдена";
        }
    }
}