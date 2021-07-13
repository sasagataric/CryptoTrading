using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptoTrading.Data.Entities;

namespace CryptoTrading.Domain.Models
{
    public class WalletHistoryModel
    {
        public Guid Id { get; set; }
        public string CoinId { get; set; }
        public CoinDomainModel Coin { get; set; }
        public Guid WalletId { get; set; }
        public DateTime TransactionDate { get; set; }
        public decimal Amount { get; set; }
        public decimal CoinPriceAtTheTime { get; set; }
    }
}
