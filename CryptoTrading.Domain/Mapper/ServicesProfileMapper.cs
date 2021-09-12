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

            CreateMap<Wallet, WalletDomainModel>()
                .ForMember(dest => dest.PurchasedCoins, opt => opt.MapFrom(src => src.PurchasedCoin))
                .ReverseMap();

            CreateMap<CoinGecko.Entities.Response.Coins.CoinMarkets, Coin>().ReverseMap();

            CreateMap<CoinDomainModel, Coin>().ReverseMap();

            CreateMap<Holding, HoldingDomainModel>().ReverseMap();

            CreateMap<WalletHistory, WalletHistoryModel>()
                .ForMember(dest => dest.CoinPriceAtTheTime, opt => opt.MapFrom(src => src.CoinPrice))
                .ReverseMap();
        }
    }
}
