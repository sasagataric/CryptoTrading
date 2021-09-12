using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoTrading.API.Models
{
    public class AddBalanceWalletModel
    {
        public Guid WalletId { get; set; }
        public decimal Amount { get; set; }
    }
}
