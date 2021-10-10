using CryptoTrading.Data.Entities;
using System;
using System.Threading.Tasks;

namespace CryptoTrading.Repositories.Interfaces
{
    public interface IWalletsRepository : IRepository<Wallet>
    {
        Task<Wallet> GetByWalletIdAsync(Guid walletId);
        Task<Wallet> GetByUserIdAsync(Guid userId);
    }
}
