using CryptoTrading.Data.Entities;
using System.Threading.Tasks;

namespace CryptoTrading.Repositories.Interfaces
{
    public interface IUsersRepository : IRepository<User>
    {
        Task<User> GetByUserNameAsync(string username);
        Task<bool> CheckUsername(string username);
        Task<bool> CheckEmail(string email);
        Task<User> GetByEmailAsync(string email);
    }
}
