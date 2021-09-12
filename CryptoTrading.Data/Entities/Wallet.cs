using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
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
        [Column(TypeName = "decimal(18,2)")]
        public decimal Balance { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Profit { get; set; }
        public ICollection<Coin> Coins { get; set; }
        public ICollection<Holding> PurchasedCoin { get; set; }
        public ICollection<WalletHistory> WalletHistorys { get; set; }
    }
}
