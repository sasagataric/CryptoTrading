using CryptoTrading.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CryptoTrading.Repositories.Interfaces
{
    public interface IHoldingsRepository
    {
        Task<IEnumerable<Holding>> GetTransactionsByUserIdAsync(Guid userId);
        Task<Holding> GetTransactionAsync(Guid walletId, string coinId);
        Holding DeletePurchasedCoin(Holding purchasedCoin);
        Task<Holding> InsertAsync(Holding purchasedCoin);
        Task SaveAsync();
    }
}
