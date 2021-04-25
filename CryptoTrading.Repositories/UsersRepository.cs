using CryptoTrading.Data.Context;
using CryptoTrading.Data.Entities;
using CryptoTrading.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTrading.Repositories
{
    public interface IUsersRepository : IRepository<User> { }
    public class UsersRepository : IUsersRepository
    {
        private CryptoTradingContext _cryptoTradingContext;
        public UsersRepository(CryptoTradingContext cryptoTradingContext)
        {
            _cryptoTradingContext = cryptoTradingContext;
        }
        public async Task<IEnumerable<User>> GetAllAsync()
        {
            var data = await _cryptoTradingContext.Users.ToListAsync();
            return data;
        }

        public async Task<User> GetByIdAsync(object id)
        {
            var data = await _cryptoTradingContext.Users.Where(user => user.Id==(Guid)id).Include(u=>u.Coins).FirstOrDefaultAsync();
            return data;
        }

        public async Task<User> InsertAsync(User obj)
        {
            var data = await _cryptoTradingContext.Users.AddAsync(obj);
            return data.Entity;
        }

        public async Task SaveAsync()
        {
            await _cryptoTradingContext.SaveChangesAsync();
        }

        public User Update(User obj)
        {
            var data = _cryptoTradingContext.Users.Update(obj);
            return data.Entity;
        }
        public User Delete(object id)
        {
            var data = _cryptoTradingContext.Users.Find(id);
            var deleted = _cryptoTradingContext.Users.Remove(data);

            return deleted.Entity;
        }

    }
}
