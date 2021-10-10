using CryptoTrading.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CryptoTrading.Repositories.Interfaces
{
    public interface ICoinsRepository : IRepository<Coin>
    {
        Task<IEnumerable<Coin>> GetCoinsFromWathchlistByUserId(Guid userId);
    }
}
