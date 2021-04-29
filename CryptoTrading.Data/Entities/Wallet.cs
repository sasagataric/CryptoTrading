using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTrading.Data.Entities
{
    public class Wallet
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public User User { get; set; }
        public double Balance { get; set; }
        public double? Profit { get; set; }
        public ICollection<Coin> Coins { get; set; }
        public ICollection<PurchasedCoin> PurchasedCoin { get; set; }
        public ICollection<WalletHistory> WalletHistorys { get; set; }
    }
}
