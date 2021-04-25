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
    public interface ICoinsRepository: IRepository<Coin> 
    {
        Task<IEnumerable<Coin>> GetCoinsFromWathchlistByUserId(Guid userId);
    }
    public class CoinsRepository : ICoinsRepository
    {
        private CryptoTradingContext _cryptoTradingContext;
        public CoinsRepository (CryptoTradingContext cryptoTradingContext)
        {
            _cryptoTradingContext = cryptoTradingContext;
        }
        public async Task<IEnumerable<Coin>> GetAllAsync()
        {
            var data =await _cryptoTradingContext.Coins.ToListAsync();
            return data;
        }

        public async Task<Coin> GetByIdAsync(object id)
        {
            var data = await _cryptoTradingContext.Coins.FindAsync(id);
            return data;
        }

        public async Task<Coin> InsertAsync(Coin obj)
        {
            var result = await _cryptoTradingContext.Coins.AddAsync(obj);
            return result.Entity;
        }

        public async Task SaveAsync()
        {
           await _cryptoTradingContext.SaveChangesAsync();
        }

        public Coin Update(Coin obj)
        {
            var result =  _cryptoTradingContext.Coins.Update(obj);
            return result.Entity;
        }
        public Coin Delete(object id)
        {
            var data = _cryptoTradingContext.Coins.Find(id);
            var deleted = _cryptoTradingContext.Coins.Remove(data);

            return deleted.Entity;
        }

        public async Task<IEnumerable<Coin>> GetCoinsFromWathchlistByUserId(Guid userId)
        {
            //var user = await _cryptoTradingContext.Users.FindAsync(userId);
            //var data = user.Coins.ToList();
            var data = await _cryptoTradingContext.Coins.Where(c => c.Users.Any(u => u.Id == userId)).ToListAsync();
            return data;
        }
    }
}
