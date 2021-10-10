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
    public class WalletHistoryRepository : IWalletHistoryRepository
    {
        private readonly CryptoTradingContext _cryptoTradingContext;
        public WalletHistoryRepository(CryptoTradingContext cryptoTradingContext)
        {
            _cryptoTradingContext = cryptoTradingContext;
        }
        public async Task<IEnumerable<WalletHistory>> GetAllAsync()
        {
            var data = await _cryptoTradingContext.WalletHistorys.Select(h=> new WalletHistory
                                                                    {
                                                                        Id = h.Id,
                                                                        Amount = h.Amount,
                                                                        CoinPrice = h.CoinPrice,
                                                                        TransactionDate = h.TransactionDate,
                                                                        CoinId = h.CoinId,
                                                                        WalletId = h.WalletId,
                                                                        Coin = new Coin
                                                                        {
                                                                            Id = h.Coin.Id,
                                                                            Image = h.Coin.Image,
                                                                            Name = h.Coin.Name,
                                                                            Symbol = h.Coin.Symbol
                                                                        }
                                                                    }).ToListAsync();
            return data;
        }

        public async Task<WalletHistory> GetByIdAsync(object id)
        {
            var data = await _cryptoTradingContext.WalletHistorys.Where(w=>w.Id==(Guid)id)
                                                                .Select(h => new WalletHistory
                                                                {
                                                                    Id = h.Id,
                                                                    Amount = h.Amount,
                                                                    CoinPrice = h.CoinPrice,
                                                                    TransactionDate = h.TransactionDate,
                                                                    CoinId = h.CoinId,
                                                                    WalletId = h.WalletId,
                                                                    Coin = new Coin
                                                                    {
                                                                        Id = h.Coin.Id,
                                                                        Image = h.Coin.Image,
                                                                        Name = h.Coin.Name,
                                                                        Symbol = h.Coin.Symbol
                                                                    }
                                                                }).OrderBy(w=>w.TransactionDate).FirstOrDefaultAsync();
            return data;
        }

        public async Task<IEnumerable<WalletHistory>> GetByUserIdAsync(Guid userId)
        {
            var data = await _cryptoTradingContext.WalletHistorys.Where(w => w.Wallet.UserId == userId).ToListAsync();
            return data;
        }

        public async Task<IEnumerable<WalletHistory>> GetByWalletIdAsync(Guid walletId)
        {
            var data = await _cryptoTradingContext.WalletHistorys
                .Where(w => w.Wallet.Id == walletId)
                    .Select(h => new WalletHistory
                    {
                        Id = h.Id,
                        Amount = h.Amount,
                        CoinPrice = h.CoinPrice,
                        TransactionDate = h.TransactionDate,
                        CoinId = h.CoinId,
                        WalletId = h.WalletId,
                        Coin = new Coin
                        {
                            Id = h.Coin.Id,
                            Image = h.Coin.Image,
                            Name = h.Coin.Name,
                            Symbol = h.Coin.Symbol
                        }
                    }).OrderByDescending(w => w.TransactionDate).ToListAsync();
            return data;
        }

        public async Task<WalletHistory> InsertAsync(WalletHistory obj)
        {
            var data = await _cryptoTradingContext.WalletHistorys.AddAsync(obj);
            return data.Entity;
        }

        public async Task SaveAsync()
        {
            await _cryptoTradingContext.SaveChangesAsync();
        }

        public WalletHistory Update(WalletHistory obj)
        {
            var data = _cryptoTradingContext.WalletHistorys.Update(obj);
            return data.Entity;
        }
        public WalletHistory Delete(object id)
        {
            var walletHistory = _cryptoTradingContext.WalletHistorys.Find(id);
            var deleted = _cryptoTradingContext.WalletHistorys.Remove(walletHistory);
            return deleted.Entity;
        }

        public async Task<IEnumerable<WalletHistory>> GetByWalletIdForCoinIdAsync(Guid walletId, string coinId)
        {
            var data = await _cryptoTradingContext.WalletHistorys
                .Where(w => w.Wallet.Id == walletId && w.CoinId == coinId)
                    .Select(h => new WalletHistory
                    {
                        Id = h.Id,
                        Amount = h.Amount,
                        CoinPrice = h.CoinPrice,
                        TransactionDate = h.TransactionDate,
                        CoinId = h.CoinId,
                        WalletId = h.WalletId,
                        Coin = new Coin
                        {
                            Id = h.Coin.Id,
                            Image = h.Coin.Image,
                            Name = h.Coin.Name,
                            Symbol = h.Coin.Symbol
                        }
                    }).OrderByDescending(w => w.TransactionDate).ToListAsync();
            return data;
        }

        public async Task<IEnumerable<WalletHistory>> GetBoughtForWalletIdAndCoinIdAfterDateAsync(Guid walletId, string coinId, DateTime date)
        {
            var data = await _cryptoTradingContext.WalletHistorys
                .Where(w => w.Wallet.Id == walletId && w.CoinId == coinId && w.TransactionDate >= date && w.Amount > 0)
                    .Select(h => new WalletHistory
                    {
                        Id = h.Id,
                        Amount = h.Amount,
                        CoinPrice = h.CoinPrice,
                        TransactionDate = h.TransactionDate,
                        CoinId = h.CoinId,
                        WalletId = h.WalletId,
                        Coin = new Coin
                        {
                            Id = h.Coin.Id,
                            Image = h.Coin.Image,
                            Name = h.Coin.Name,
                            Symbol = h.Coin.Symbol
                        }
                    }).OrderByDescending(w => w.TransactionDate).ToListAsync();
            return data;
        }

        public async Task<IEnumerable<WalletHistory>> GetByWalletIdInRangeAsync(Guid walletId, DateTime start, DateTime end)
        {
            var data = await _cryptoTradingContext.WalletHistorys
                .Where(w => w.Wallet.Id == walletId && w.TransactionDate.Date >= start.Date && w.TransactionDate.Date <= end.Date)
                    .Select(h => new WalletHistory
                    {
                        Id = h.Id,
                        Amount = h.Amount,
                        CoinPrice = h.CoinPrice,
                        TransactionDate = h.TransactionDate,
                        CoinId = h.CoinId,
                        WalletId = h.WalletId,
                        Coin = new Coin
                        {
                            Id = h.Coin.Id,
                            Image = h.Coin.Image,
                            Name = h.Coin.Name,
                            Symbol = h.Coin.Symbol
                        }
                    }).OrderByDescending(w => w.TransactionDate).ToListAsync();
            return data;
        }
    }
}
