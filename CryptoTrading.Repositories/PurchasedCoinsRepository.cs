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
    public interface IPurchasedCoinsRepository
    {
        Task<IEnumerable<PurchasedCoin>> GetPurchasedCoinsByUserIdAsync(Guid userId);
        Task<PurchasedCoin> GetPurchasedCoinAsync(Guid walletId, string coinId);
        PurchasedCoin DeletePurchasedCoin(PurchasedCoin purchasedCoin);
        Task<PurchasedCoin> InsertAsync(PurchasedCoin purchasedCoin);
        Task SaveAsync();
    }
    public class PurchasedCoinsRepository : IPurchasedCoinsRepository
    {
        private readonly CryptoTradingContext _cryptoTradingContext;
        public PurchasedCoinsRepository(CryptoTradingContext cryptoTradingContext)
        {
            _cryptoTradingContext = cryptoTradingContext;
        }

        public PurchasedCoin DeletePurchasedCoin(PurchasedCoin purchasedCoin)
        {
            var deleted = _cryptoTradingContext.PurchasedCoin.Remove(purchasedCoin);
            return deleted.Entity;
        }

        public async Task<IEnumerable<PurchasedCoin>> GetPurchasedCoinsByUserIdAsync(Guid userId)
        {
            var purchasedCoin = await _cryptoTradingContext.PurchasedCoin.Where(x => x.Wallet.UserId == userId )
                                                                                    .Include(x =>x.Wallet)
                                                                                    .Include(x=>x.Coin)
                                                                                    .ToArrayAsync();
            return purchasedCoin;
        }

        public async Task<PurchasedCoin> GetPurchasedCoinAsync(Guid walletId, string coinId)
        {
            var purchasedCoin = await _cryptoTradingContext.PurchasedCoin.Where(x => x.CoinId == coinId && x.WalletId == walletId)
                                                                        .Include(x => x.Wallet)
                                                                        .Include(x => x.Coin)
                                                                        .FirstOrDefaultAsync();
            return purchasedCoin;
        }

        public async Task<PurchasedCoin> InsertAsync(PurchasedCoin purchasedCoin)
        {
            var purchase = await _cryptoTradingContext.PurchasedCoin.AddAsync(purchasedCoin);
            return purchase.Entity;
        }

        public async Task SaveAsync()
        {
            await _cryptoTradingContext.SaveChangesAsync();
        }
    }
}
