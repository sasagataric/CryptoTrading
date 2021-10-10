using System.Collections.Generic;
using System.Threading.Tasks;

namespace CryptoTrading.Repositories.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(object id);
        Task<T> InsertAsync(T obj);
        Task SaveAsync();
        T Update(T obj);
        T Delete(object id);
    }
}
