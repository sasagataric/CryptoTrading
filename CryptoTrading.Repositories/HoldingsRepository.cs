using CryptoTrading.Data.Context;
using CryptoTrading.Data.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTrading.Repositories
{
    public interface IHoldingsRepository
    {
        Task<IEnumerable<Holding>> GetTransactionsByUserIdAsync(Guid userId);
        Task<Holding> GetTransactionAsync(Guid walletId, string coinId);
        Holding DeletePurchasedCoin(Holding purchasedCoin);
        Task<Holding> InsertAsync(Holding purchasedCoin);
        Task SaveAsync();
    }
    public class HoldingsRepository : IHoldingsRepository
    {
        private readonly CryptoTradingContext _cryptoTradingContext;
        public HoldingsRepository(CryptoTradingContext cryptoTradingContext)
        {
            _cryptoTradingContext = cryptoTradingContext;
        }

        public Holding DeletePurchasedCoin(Holding purchasedCoin)
        {
            var deleted = _cryptoTradingContext.Holdings.Remove(purchasedCoin);
            return deleted.Entity;
        }

        public async Task<IEnumerable<Holding>> GetTransactionsByUserIdAsync(Guid userId)
        {
            var purchasedCoin = await _cryptoTradingContext.Holdings.Where(x => x.Wallet.UserId == userId )
                                                                                    .Include(x =>x.Wallet)
                                                                                    .Include(x=>x.Coin)
                                                                                    .ToArrayAsync();
            return purchasedCoin;
        }

        public async Task<Holding> GetTransactionAsync(Guid walletId, string coinId)
        {
            var purchasedCoin = await _cryptoTradingContext.Holdings.Where(x => x.CoinId == coinId && x.WalletId == walletId)
                                                                        .Include(x => x.Wallet)
                                                                        .Include(x => x.Coin)
                                                                        .FirstOrDefaultAsync();
            return purchasedCoin;
        }

        public async Task<Holding> InsertAsync(Holding purchasedCoin)
        {
            var purchase = await _cryptoTradingContext.Holdings.AddAsync(purchasedCoin);
            return purchase.Entity;
        }

        public async Task SaveAsync()
        {
            await _cryptoTradingContext.SaveChangesAsync();
        }
    }
}
