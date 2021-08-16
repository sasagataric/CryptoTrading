using CryptoTrading.IdentityServer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoTrading.IdentityServer.Repository.Interfaces
{
    public interface IProviderRepository
    {
        IEnumerable<Provider> Get();
    }
}
