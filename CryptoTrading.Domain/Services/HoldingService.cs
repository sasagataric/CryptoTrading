using AutoMapper;
using CryptoTrading.Data.Entities;
using CryptoTrading.Domain.Common;
using CryptoTrading.Domain.Interfaces;
using CryptoTrading.Domain.Models;
using CryptoTrading.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CryptoTrading.Domain.Services
{
    public class HoldingService : IHoldingService
    {
        private readonly IWalletsRepository _walletRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly IHoldingsRepository _holdingsRepository;
        private readonly ICoinsRepository _coinsRepository;
        private readonly CoinGecko.Interfaces.ICoinGeckoClient _coinGeckoClient;
        private readonly IMapper _mapper;
        private readonly IWalletHistoryRepository _walletHistoryRepository;

        public HoldingService(IWalletsRepository walletRepository,
                                    IHoldingsRepository holdingsRepository,
                                    ICoinsRepository coinsRepository,
                                    CoinGecko.Interfaces.ICoinGeckoClient coinGeckoClient,
                                    IMapper mapper, 
                                    IUsersRepository usersRepository, IWalletHistoryRepository walletHistoryRepository)
        {
            _walletRepository = walletRepository;
            _holdingsRepository = holdingsRepository;
            _coinsRepository = coinsRepository;
            _coinGeckoClient = coinGeckoClient;
            _mapper = mapper;
            _usersRepository = usersRepository;
            _walletHistoryRepository = walletHistoryRepository;
        }

        public async Task<GenericDomainModel<HoldingDomainModel>> BuyCoinAsync(Guid walletId, string coinId, decimal coinAmount)
        {
            var wallet = await _walletRepository.GetByIdAsync(walletId);
            if (wallet == null)
            {
                return new GenericDomainModel<HoldingDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.WALLET_ID_NULL
                };
            }

            var coin = await _coinsRepository.GetByIdAsync(coinId);

            var coinCurrantData = await _coinGeckoClient.CoinsClient.GetCoinMarkets("eur", new[] { coinId }, "market_cap_desc", null, null, false, "", "");
            if (coinCurrantData == null)
            {
                return new GenericDomainModel<HoldingDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.COINGECKO_COIN_DATA_ERROR
                };
            }

            var currentPrice = coinCurrantData[0].CurrentPrice;
            if (currentPrice != null && wallet.Balance < currentPrice * coinAmount)
            {
                return new GenericDomainModel<HoldingDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.WALLET_INSUFFICIENT_FOUNDS
                };
            }

            var price = coinCurrantData[0].CurrentPrice;
            if (price != null)
                wallet.Balance -= price.Value * coinAmount;

            var curranDateTime = DateTime.Now;

            var checkHolding = await _holdingsRepository.GetHoldingAsync(walletId, coinId);
            if (checkHolding != null)
            {
                checkHolding.Amount += coinAmount;
                checkHolding.AverageBuyingPrice =await CalculateNewAverageBuyingPrice(walletId, coinId, coinAmount, price.Value, checkHolding.DateOfFirstPurchase);
            }
            else
            {
                var newPurchase = new Holding
                {
                    Wallet = wallet,
                    WalletId = walletId,
                    Coin = coin ?? _mapper.Map<Coin>(coinCurrantData[0]),
                    CoinId = coinId,
                    Amount = coinAmount,
                    DateOfFirstPurchase = curranDateTime,
                    AverageBuyingPrice= await CalculateNewAverageBuyingPrice(walletId, coinId, coinAmount, price.Value, curranDateTime),
                    AverageSellingPrice =0
                };

                checkHolding = await _holdingsRepository.InsertAsync(newPurchase);
                if (checkHolding == null)
                {
                    return new GenericDomainModel<HoldingDomainModel>
                    {
                        IsSuccessful = false,
                        ErrorMessage = Messages.PURCHASED_COIN_CREATION_ERROR
                    };
                }
            }

            var histoy = new WalletHistory
            {
                TransactionDate = curranDateTime,
                WalletId = walletId,
                CoinId = coinId,
                CoinPrice = coinCurrantData[0].CurrentPrice ?? 0,
                Amount = coinAmount
            };

            await _walletHistoryRepository.InsertAsync(histoy);


            await _holdingsRepository.SaveAsync();

            return new GenericDomainModel<HoldingDomainModel>
            {
                IsSuccessful = true,
                Data = _mapper.Map<HoldingDomainModel>(checkHolding)
            };
        }

        public async Task<decimal> CalculateNewAverageBuyingPrice(Guid walletId, string coinId, decimal coinAmount, decimal coinPrice ,DateTime date)
        {
            decimal totalBought = coinAmount * coinPrice;
            decimal totalCoinAount = coinAmount;

            var history =await _walletHistoryRepository.GetBoughtForWalletIdAndCoinIdAfterDateAsync(walletId, coinId, date);
            if (history == null) return totalBought / totalCoinAount;
            
            foreach(var wh in history)
            {
                totalBought += wh.Amount * wh.CoinPrice;
                totalCoinAount += wh.Amount;
            }
            return totalBought / totalCoinAount;
        }

        public async Task<GenericDomainModel<HoldingDomainModel>> SellCoinAsync(Guid walletId, string coinId, decimal coinAmount)
        {
            var wallet = await _walletRepository.GetByIdAsync(walletId);
            if (wallet == null)
            {
                return new GenericDomainModel<HoldingDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.WALLET_ID_NULL
                };
            }

            var holding= await _holdingsRepository.GetHoldingAsync(walletId, coinId);
            if (holding == null)
            {
                return new GenericDomainModel<HoldingDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.PURCHASED_COIN_CANT_BE_SOLD
                };
            }

            var coinCurrantData = await _coinGeckoClient.CoinsClient.GetCoinMarkets("eur", new[] { coinId }, "market_cap_desc", null, null, false, "", "");
            if (coinCurrantData == null)
            {
                return new GenericDomainModel<HoldingDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.COINGECKO_COIN_DATA_ERROR
                };
            }

            if (holding.Amount < coinAmount)
            {
                return new GenericDomainModel<HoldingDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.PURCHASED_COIN_NOT_ENOUGHT_COINS_IN_WALLET
                };
            }

            holding.Amount -= coinAmount;

            if (coinCurrantData[0].CurrentPrice != null)
                wallet.Balance += coinAmount * coinCurrantData[0].CurrentPrice.Value;


            var histoy = new WalletHistory
            {
                TransactionDate = DateTime.Now,
                WalletId = walletId,
                CoinId = coinId,
                CoinPrice = coinCurrantData[0].CurrentPrice ?? 0,
                Amount = -coinAmount
            };

            await _walletHistoryRepository.InsertAsync(histoy);


            if (holding.Amount == 0)
            {
                holding = _holdingsRepository.DeleteHoldingCoin(holding);
            }
            await _holdingsRepository.SaveAsync();

            return new GenericDomainModel<HoldingDomainModel>
            {
                IsSuccessful = true,
                Data = _mapper.Map<HoldingDomainModel>(holding)
            };
        }

        public async Task<GenericDomainModel<HoldingDomainModel>> GetHoldingsByUserId(Guid userId)
        {
            var user =await _usersRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return new GenericDomainModel<HoldingDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.USER_ID_ERROR
                };
            }

            var holdings =await _holdingsRepository.GetHoldingsByUserIdAsync(userId);
            if (holdings == null)
            {
                return new GenericDomainModel<HoldingDomainModel>
                {
                    IsSuccessful = true,
                };
            }

            return new GenericDomainModel<HoldingDomainModel>
            {
                IsSuccessful = true,
                DataList = _mapper.Map<IEnumerable<Holding>, List<HoldingDomainModel>>(holdings)
            };

        }

        public async Task<GenericDomainModel<HoldingDomainModel>> GetHolding(Guid walletId, string coinId)
        {
            var wallet = await _walletRepository.GetByIdAsync(walletId);
            if (wallet == null)
            {
                return new GenericDomainModel<HoldingDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.WALLET_ID_NULL
                };
            }

            var holdings =await _holdingsRepository.GetHoldingAsync(walletId, coinId);
            if (holdings == null)
            {
                return new GenericDomainModel<HoldingDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.PURCHASED_CANT_BE_FOUND
                };
            }

            return new GenericDomainModel<HoldingDomainModel>
            {
                IsSuccessful = true,
                Data = _mapper.Map<HoldingDomainModel>(holdings)
            };

        }
    }
}
