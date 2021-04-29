using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTrading.Data.Entities
{
    public class Coin
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Symbol { get; set; }
        public ICollection<Wallet> Wallets { get; set; }
        public ICollection<User> Users { get; set; }
        public ICollection<PurchasedCoin> PurchasedCoin { get; set; }
        public ICollection<WalletHistory> WalletHistorys { get; set; }

    }
}
