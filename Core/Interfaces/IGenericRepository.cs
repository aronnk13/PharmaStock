
namespace PharmaStock.Core.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T?> GetByIdAsync(int id);
        Task AddAsync(T obj);   
        void Update(T obj);
        Task DeleteAsync(int id);
    }
}