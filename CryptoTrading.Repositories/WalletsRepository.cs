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
    public interface IWalletsRepositor : IRepository<Wallet>
    {
        Task<Wallet> GetByWalletIdAsync(Guid walletId);
        Task<Wallet> GetByUserIdAsync(Guid userId);
    }
    public class WalletsRepository : IWalletsRepositor
    {
        private CryptoTradingContext _cryptoTradingContext;
        public WalletsRepository(CryptoTradingContext cryptoTradingContext)
        {
            _cryptoTradingContext = cryptoTradingContext;
        }
        public async Task<IEnumerable<Wallet>> GetAllAsync()
        {
            var data = await _cryptoTradingContext.Wallets.Include(w => w.PurchasedCoin).ToListAsync();
            return data;
        }

        public async Task<Wallet> GetByIdAsync(object id)
        {
            var data = await _cryptoTradingContext.Wallets.Where(Wallet => Wallet.Id==(Guid)id).Include(w=>w.PurchasedCoin).FirstOrDefaultAsync();
            return data;
        }

        public async Task<Wallet> GetByWalletIdAsync(Guid walletId)
        {
            var data = await _cryptoTradingContext.Wallets.Where(Wallet => Wallet.Id == walletId).Include(w => w.PurchasedCoin).FirstOrDefaultAsync();
            return data;
        }

        public async Task<Wallet> InsertAsync(Wallet obj)
        {
            var data = await _cryptoTradingContext.Wallets.AddAsync(obj);
            return data.Entity;
        }

        public async Task SaveAsync()
        {
            await _cryptoTradingContext.SaveChangesAsync();
        }

        public Wallet Update(Wallet obj)
        {
            var data = _cryptoTradingContext.Wallets.Update(obj);
            return data.Entity;
        }
        public Wallet Delete(object id)
        {
            var data = _cryptoTradingContext.Wallets.Find(id);
            var deleted = _cryptoTradingContext.Wallets.Remove(data);

            return deleted.Entity;
        }

        public async Task<Wallet> GetByUserIdAsync(Guid userId)
        {
            var data = await _cryptoTradingContext.Wallets.Where(Wallet => Wallet.UserId == userId).Include(w => w.PurchasedCoin).FirstOrDefaultAsync();
            return data;
        }
    }
}
