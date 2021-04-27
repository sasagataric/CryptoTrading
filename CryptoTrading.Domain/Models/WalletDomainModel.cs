using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTrading.Domain.Models
{
    public class WalletDomainModel
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public double Balance { get; set; }
        public double? Profit { get; set; }
    }
}
