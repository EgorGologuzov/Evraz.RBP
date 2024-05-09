namespace RBP.Services.Interfaces
{
    public interface IValidator<T> where T : class
    {
        void Validate(T entity);
    }
}