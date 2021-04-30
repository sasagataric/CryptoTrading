using System;
using System.ComponentModel.DataAnnotations;


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
        [Range(0, double.MaxValue, ErrorMessage = "Please enter Amount bigger than {0}")]
        public double Amount { get; set; }
    }
}
