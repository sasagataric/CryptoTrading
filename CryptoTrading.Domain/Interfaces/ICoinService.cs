using CryptoTrading.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTrading.Domain.Interfaces
{
    public interface ICoinService
    {
        Task<GenericDomainModel<CoinDomainModel>> GetByIdAsync(string coinId);
        Task<GenericDomainModel<CoinDomainModel>> DeleteVoinAsync(string coinId);
        Task<GenericDomainModel<CoinDomainModel>> CreateCoinAsync(string coinId);
    }
}
