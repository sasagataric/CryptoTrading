using System;

namespace CryptoTrading.API.Models
{
    public class CreateWalletModel
    {
        public Guid UserId { get; set; }
        public decimal Balance { get; set; }
    }
}
