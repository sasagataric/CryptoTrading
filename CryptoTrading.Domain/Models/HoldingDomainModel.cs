using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTrading.Domain.Models
{
    public class HoldingDomainModel
    {
        public CoinDomainModel Coin { get; set; }
        public Guid WalletId { get; set; }
        public decimal Amount { get; set; }
        public decimal AverageBuyingPrice { get; set; }
        public decimal AverageSellingPrice { get; set; }
        public DateTime DateOfFirstPurchase { get; set; }
    }
}
