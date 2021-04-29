using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoTrading.API.Models
{
    public class PurchaseCoinModel
    {
        [Required]
        [RegularExpression("^((?!00000000-0000-0000-0000-000000000000).)*$", ErrorMessage = "Cannot use default Guid")]
        public Guid WalletId { get; set; }

        [Required]
        public string CoinId { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Please enter amount bigger than {0}")]
        public double Amount { get; set; }
    }
}
