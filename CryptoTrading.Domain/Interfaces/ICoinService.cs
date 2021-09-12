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
        Task<GenericDomainModel<CoinDomainModel>> DeleteCoinAsync(string coinId);
        Task<GenericDomainModel<CoinDomainModel>> CreateCoinAsync(string coinId);
        Task<GenericDomainModel<CoinGecko.Entities.Response.Coins.CoinMarkets>> GetWatchListByUserIdAsync(Guid userId);
        Task<GenericDomainModel<CoinDomainModel>> AddCoinToWatchListAsync(Guid userId,string coinId);
        Task<GenericDomainModel<CoinDomainModel>> RemoveCoinFromWatchListAsync(Guid userId, string coinId);


    }
}
