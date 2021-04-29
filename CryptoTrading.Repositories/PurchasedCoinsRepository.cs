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
        Task<PurchasedCoin> GetPurchasedCoin(Guid walletId, string coinId);
        Task<PurchasedCoin> DeletePurchasedCoin(Guid walletId, string coinId);
        Task<PurchasedCoin> Insert(PurchasedCoin PurchasedCoin);
        Task SaveAsync();
    }
    public class PurchasedCoinsRepository : IPurchasedCoinsRepository
    {
        private CryptoTradingContext _cryptoTradingContext;
        public PurchasedCoinsRepository(CryptoTradingContext cryptoTradingContext)
        {
            _cryptoTradingContext = cryptoTradingContext;
        }

        public async Task<PurchasedCoin> DeletePurchasedCoin(Guid walletId, string coinId)
        {
            var purchasedCoin = await _cryptoTradingContext.PurchasedCoin.Where(x => x.CoinId == coinId && x.WalletId == walletId).FirstOrDefaultAsync();
            var deleted = _cryptoTradingContext.PurchasedCoin.Remove(purchasedCoin);
            return deleted.Entity;
        }

        public async Task<PurchasedCoin> GetPurchasedCoin(Guid walletId, string coinId)
        {
            var purchasedCoin = await _cryptoTradingContext.PurchasedCoin.Where(x => x.CoinId == coinId && x.WalletId == walletId).FirstOrDefaultAsync();
            return purchasedCoin;
        }

        public async Task<PurchasedCoin> Insert(PurchasedCoin PurchasedCoin)
        {
            var purchasedCoin = await _cryptoTradingContext.PurchasedCoin.AddAsync(PurchasedCoin);
            return purchasedCoin.Entity;
        }

        public async Task SaveAsync()
        {
            await _cryptoTradingContext.SaveChangesAsync();
        }
    }
}
