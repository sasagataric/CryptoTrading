using CryptoTrading.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CryptoTrading.Repositories.Interfaces
{
    public interface IHoldingsRepository
    {
        Task<IEnumerable<Holding>> GetHoldingsByUserIdAsync(Guid userId);
        Task<Holding> GetHoldingAsync(Guid walletId, string coinId);
        Holding DeleteHoldingCoin(Holding purchasedCoin);
        Task<Holding> InsertAsync(Holding purchasedCoin);
        Task SaveAsync();
    }
}
