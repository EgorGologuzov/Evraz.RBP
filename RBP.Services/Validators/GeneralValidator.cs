using RBP.Services.Interfaces;

namespace RBP.Services.Validators
{
    public class GeneralValidator<T> : IValidator<T> where T : class
    {
        public virtual void Validate(T entity)
        {
        }
    }
}