using AutoMapper;
using CryptoTrading.Data.Entities;
using CryptoTrading.Domain.Common;
using CryptoTrading.Domain.Interfaces;
using CryptoTrading.Domain.Models;
using CryptoTrading.Repositories;
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

            var checkPurchasedCoin = await _holdingsRepository.GetTransactionAsync(walletId, coinId);
            if (checkPurchasedCoin != null)
            {
                checkPurchasedCoin.Amount += coinAmount;
                checkPurchasedCoin.AverageBuyingPrice =await CalculateNewAverageBuyingPrice(walletId, coinId, coinAmount, price.Value, checkPurchasedCoin.DateOfFirstPurchase);
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

                checkPurchasedCoin = await _holdingsRepository.InsertAsync(newPurchase);
                if (checkPurchasedCoin == null)
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
                Data = _mapper.Map<HoldingDomainModel>(checkPurchasedCoin)
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

            var purchasedCoin= await _holdingsRepository.GetTransactionAsync(walletId, coinId);
            if (purchasedCoin == null)
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

            if (purchasedCoin.Amount < coinAmount)
            {
                return new GenericDomainModel<HoldingDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.PURCHASED_COIN_NOT_ENOUGHT_COINS_IN_WALLET
                };
            }

            purchasedCoin.Amount -= coinAmount;

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


            if (purchasedCoin.Amount == 0)
            {
                purchasedCoin = _holdingsRepository.DeletePurchasedCoin(purchasedCoin);
            }
            await _holdingsRepository.SaveAsync();

            return new GenericDomainModel<HoldingDomainModel>
            {
                IsSuccessful = true,
                Data = _mapper.Map<HoldingDomainModel>(purchasedCoin)
            };
        }

        public async Task<GenericDomainModel<HoldingDomainModel>> GetPurchasesByUserId(Guid userId)
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

            var purchases =await _holdingsRepository.GetTransactionsByUserIdAsync(userId);
            if (purchases == null)
            {
                return new GenericDomainModel<HoldingDomainModel>
                {
                    IsSuccessful = true,
                };
            }

            return new GenericDomainModel<HoldingDomainModel>
            {
                IsSuccessful = true,
                DataList = _mapper.Map<IEnumerable<Holding>, List<HoldingDomainModel>>(purchases)
            };

        }

        public async Task<GenericDomainModel<HoldingDomainModel>> GetPurchase(Guid walletId, string coinId)
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

            var purchase =await _holdingsRepository.GetTransactionAsync(walletId, coinId);
            if (purchase == null)
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
                Data = _mapper.Map<HoldingDomainModel>(purchase)
            };

        }
    }
}
