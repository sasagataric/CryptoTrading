using CryptoTrading.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTrading.Domain.Interfaces
{
    public interface IPurchasedCoinService
    {
        Task<GenericDomainModel<PurchasedCoinDomainModel>> BuyCoinAsync(Guid walletId, string coinId, double coinAmount);
    }
}
