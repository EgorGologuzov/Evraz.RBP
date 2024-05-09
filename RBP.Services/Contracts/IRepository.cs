namespace RBP.Services.Contracts
{
    public interface IRepository<T> where T : class
    {
        Task<T?> Get(object id);
        Task<T> Create(T entity);
        Task<T> Update(object id, object newData);
        Task<T> Delete(object id);
        Task<bool> IsExists(object id);
    }
}