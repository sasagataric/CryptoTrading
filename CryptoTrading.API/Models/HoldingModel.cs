using System;
using System.ComponentModel.DataAnnotations;


namespace CryptoTrading.API.Models
{
    public class HoldingModel
    {
        [Required]
        [RegularExpression("^((?!00000000-0000-0000-0000-000000000000).)*$", ErrorMessage = "Cannot use default Guid")]
        public Guid WalletId { get; set; }

        [Required]
        public string CoinId { get; set; }

        [Required]
        //[RegularExpression(@"^\d+\.\d{0,10}$", ErrorMessage = "It cannot have more than one decimal point value")]
        [Range(0, 9999999999999999.9999999999, ErrorMessage = "Please enter Amount bigger than {0}")]
        public decimal Amount { get; set; }
    }
}
