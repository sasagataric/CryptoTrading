using CryptoTrading.IdentityServer.Entity;
using CryptoTrading.IdentityServer.Helpers;
using CryptoTrading.IdentityServer.Repository.Interfaces;
using System.Collections.Generic;

namespace CryptoTrading.IdentityServer.Repository
{
    public class ProviderRepository : IProviderRepository
    {
        public IEnumerable<Provider> Get()
        {
            return ProviderDataSource.GetProviders();
        }
    }
}
