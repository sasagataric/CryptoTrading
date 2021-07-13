using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptoTrading.Data.Entities;

namespace CryptoTrading.Domain.Models
{
    public class WalletDomainModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public decimal Balance { get; set; }
        public decimal Profit { get; set; }
        public List<PurchasedCoinDomainModel> PurchasedCoins { get; set; }
    }
}
