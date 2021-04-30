using AutoMapper;
using CryptoTrading.Data.Entities;
using CryptoTrading.Domain.Models;

namespace CryptoTrading.Domain.Mapper
{
    public class ServicesProfileMapper : Profile
    {
        public ServicesProfileMapper()
        {
            CreateMap<User, UserDomainModel>().ReverseMap();
            CreateMap<Wallet, WalletDomainModel>().ReverseMap();
            CreateMap<CoinGecko.Entities.Response.Coins.CoinMarkets, Coin>().ReverseMap();
            CreateMap<CoinDomainModel, Coin>().ReverseMap();
            CreateMap<PurchasedCoin, PurchasedCoinDomainModel>().ReverseMap();
        }
    }
}
