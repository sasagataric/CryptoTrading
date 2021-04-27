using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoTrading.API.Models
{
    public class CreateWalletModel
    {
        public Guid UserId { get; set; }
        public Guid Balance { get; set; }
    }
}
