using CryptoTrading.IdentityServer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoTrading.IdentityServer.Interfaces
{
    public interface IGoogleAuthProvider : IExternalAuthProvider
    {
        Provider Provider { get; }
    }
}
