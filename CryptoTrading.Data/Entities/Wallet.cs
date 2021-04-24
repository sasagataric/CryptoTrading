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
        public string UserId { get; set; }
        public User User { get; set; }
        public double Balance { get; set; }
        public double? Profit { get; set; }
        public ICollection<Coin> Coins { get; set; }
        public ICollection<WalletCoins> WalletCoins { get; set; }
    }
}
