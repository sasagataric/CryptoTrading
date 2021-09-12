using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptoTrading.Domain.Models;

namespace CryptoTrading.Domain.Interfaces
{
    public interface IWalletHistoryService
    {
        Task<GenericDomainModel<WalletHistoryModel>> GetAllAsync();
        Task<GenericDomainModel<WalletHistoryModel>> GetByWalletIdAsync(Guid walletId);
        Task<GenericDomainModel<WalletHistoryModel>> GetByWalletIdInRangeAsync(Guid walletId, DateTime startDate, DateTime endDate);
        Task<GenericDomainModel<WalletHistoryModel>> GetByWalletIdForCoinIdAsync(Guid walletId, string coinId);

    }
}
