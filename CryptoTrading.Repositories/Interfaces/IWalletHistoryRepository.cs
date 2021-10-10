using CryptoTrading.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CryptoTrading.Repositories.Interfaces
{
    public interface IWalletHistoryRepository : IRepository<WalletHistory>
    {
        Task<IEnumerable<WalletHistory>> GetByUserIdAsync(Guid userId);
        Task<IEnumerable<WalletHistory>> GetByWalletIdInRangeAsync(Guid walletId, DateTime start, DateTime end);
        Task<IEnumerable<WalletHistory>> GetByWalletIdAsync(Guid walletId);
        Task<IEnumerable<WalletHistory>> GetByWalletIdForCoinIdAsync(Guid walletId, string coinId);
        Task<IEnumerable<WalletHistory>> GetBoughtForWalletIdAndCoinIdAfterDateAsync(Guid walletId, string coinId, DateTime date);

    }
}
