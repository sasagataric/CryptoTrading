using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTrading.Data.Entities
{
    public class WalletHistory
    {
        public Guid Id { get; set; }
        public string CoinId { get; set; }
        public Coin Coin { get; set; }
        public Guid WalletId { get; set; }
        public Wallet Wallet { get; set; }
        public DateTime TransactionDate { get; set; }
        public double Amount { get; set; }
        public double CoinPrice { get; set; }
    }
}
