using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoTrading.API.Models
{
    public class AddCoinToWatchListModel
    {
        [Required]
        [RegularExpression("^((?!00000000-0000-0000-0000-000000000000).)*$", ErrorMessage = "Cannot use default Guid")]
        public Guid UserId { get; set; }

        [Required]
        public string CoinId { get; set; }
    }
}
