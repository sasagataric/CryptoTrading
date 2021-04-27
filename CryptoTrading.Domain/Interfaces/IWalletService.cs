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
        Task<GenericDomainModel<WalletDomainModel>> CreateWallet(WalletDomainModel newWallet);
        Task<GenericDomainModel<WalletDomainModel>> GetWalletByUserId(Guid userId);
        Task<GenericDomainModel<WalletDomainModel>> GetById(Guid walletId);
        Task<GenericDomainModel<WalletDomainModel>> UpdateWalletBalanceAsync(Guid userId, double balanceChange);
    }
}
