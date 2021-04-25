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
    public interface IWalletssRepositor : IRepository<Wallet>
    {
        Task<Wallet> GetByWalletId(Guid WalletId);
    }
    public class WalletsRepository : IWalletssRepositor
    {
        private CryptoTradingContext _cryptoTradingContext;
        public WalletsRepository(CryptoTradingContext cryptoTradingContext)
        {
            _cryptoTradingContext = cryptoTradingContext;
        }
        public async Task<IEnumerable<Wallet>> GetAllAsync()
        {
            var data = await _cryptoTradingContext.Wallets.ToListAsync();
            return data;
        }

        public async Task<Wallet> GetByIdAsync(object id)
        {
            var data = await _cryptoTradingContext.Wallets.Where(Wallet => Wallet.Id==(Guid)id).FirstOrDefaultAsync();
            return data;
        }

        public async Task<Wallet> GetByWalletId(Guid WalletId)
        {
            var data = await _cryptoTradingContext.Wallets.Where(Wallet => Wallet.Id == WalletId).FirstOrDefaultAsync();
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

    }
}
