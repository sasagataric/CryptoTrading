using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTrading.Data.Entities
{
    public class PurchasedCoin
    {
        public string CoinId { get; set; }
        public Coin Coin { get; set; }
        public Guid WalletId { get; set; }
        public Wallet Wallet { get; set; }
        [Column(TypeName = "decimal(18,10)")]
        public decimal Amount { get; set; }
    }
}
