using CryptoTrading.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTrading.Domain.Interfaces
{
    public interface IWalletService
    {
        Task<GenericDomainModel<WalletDomainModel>> CreateWalletAsync(WalletDomainModel newWallet);
        Task<GenericDomainModel<WalletDomainModel>> GetWalletByUserIdAsync(Guid userId);
        Task<GenericDomainModel<WalletDomainModel>> GetByIdAsync(Guid walletId);
        Task<GenericDomainModel<WalletDomainModel>> UpdateWalletBalanceAsync(Guid userId, decimal balanceChange);
        Task<GenericDomainModel<WalletDomainModel>> AddBalance(Guid walletId, decimal amount);

    }
}
