using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTrading.Domain.Models
{
    public class PurchasedCoinDomainModel
    {
        public string CoinId { get; set; }
        public CoinDomainModel Coin { get; set; }
        public Guid WalletId { get; set; }
        public WalletDomainModel Wallet { get; set; }
        public decimal Amount { get; set; }
    }
}
