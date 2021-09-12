using CryptoTrading.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTrading.Domain.Interfaces
{
    public interface IHoldingService
    {
        Task<GenericDomainModel<HoldingDomainModel>> BuyCoinAsync(Guid walletId, string coinId, decimal coinAmount);
        Task<GenericDomainModel<HoldingDomainModel>> SellCoinAsync(Guid walletId, string coinId, decimal coinAmount);
        Task<GenericDomainModel<HoldingDomainModel>> GetPurchasesByUserId(Guid userId);
        Task<GenericDomainModel<HoldingDomainModel>> GetPurchase(Guid walletId, string coinId);
    }
}
