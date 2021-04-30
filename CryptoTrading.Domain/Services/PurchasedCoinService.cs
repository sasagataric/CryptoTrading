﻿using AutoMapper;
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
    public class PurchasedCoinService : IPurchasedCoinService
    {
        private readonly IWalletsRepositor _walletRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly IPurchasedCoinsRepository _purchasedCoinsRepository;
        private readonly ICoinsRepository _coinsRepository;
        private readonly CoinGecko.Interfaces.ICoinGeckoClient _coinGeckoClient;
        private readonly IMapper _mapper;

        public PurchasedCoinService(IWalletsRepositor walletRepository,
                                    IPurchasedCoinsRepository purchasedCoinsRepository,
                                    ICoinsRepository coinsRepository,
                                    CoinGecko.Interfaces.ICoinGeckoClient coinGeckoClient,
                                    IMapper mapper, 
                                    IUsersRepository usersRepository)
        {
            _walletRepository = walletRepository;

            _purchasedCoinsRepository = purchasedCoinsRepository;
            _coinsRepository = coinsRepository;
            _coinGeckoClient = coinGeckoClient;
            _mapper = mapper;
            _usersRepository = usersRepository;
        }

        public async Task<GenericDomainModel<PurchasedCoinDomainModel>> BuyCoinAsync(Guid walletId, string coinId, double coinAmount)
        {
            var wallet = await _walletRepository.GetByIdAsync(walletId);
            if (wallet == null)
            {
                return new GenericDomainModel<PurchasedCoinDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.WALLET_ID_NULL
                };
            }

            var coin = await _coinsRepository.GetByIdAsync(coinId);

            var coinCurrantData = await _coinGeckoClient.CoinsClient.GetCoinMarkets("eur", new[] { coinId }, "market_cap_desc", 3, 1, false, "", "");
            if (coinCurrantData == null)
            {
                return new GenericDomainModel<PurchasedCoinDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.COINGECKO_COIN_DATA_ERROR
                };
            }

            var currentPrice = coinCurrantData[0].CurrentPrice;
            if (currentPrice != null && wallet.Balance < (double)currentPrice * coinAmount)
            {
                return new GenericDomainModel<PurchasedCoinDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.WALLET_INSUFFICIENT_FOUNDS
                };
            }

            var checkPurchasedCoin = await _purchasedCoinsRepository.GetPurchasedCoinAsync(walletId, coinId);
            if (checkPurchasedCoin != null)
            {
                checkPurchasedCoin.Amount += coinAmount;
            }
            else
            {
                var newPurchase = new PurchasedCoin
                {
                    Wallet = wallet,
                    WalletId = walletId,
                    Coin = coin ?? _mapper.Map<Coin>(coinCurrantData[0]),
                    CoinId = coinId,
                    Amount = coinAmount
                };

                checkPurchasedCoin = await _purchasedCoinsRepository.InsertAsync(newPurchase);
                if (checkPurchasedCoin == null)
                {
                    return new GenericDomainModel<PurchasedCoinDomainModel>
                    {
                        IsSuccessful = false,
                        ErrorMessage = Messages.PURCHASED_COIN_CREATION_ERROR
                    };
                }
            }

            var price = coinCurrantData[0].CurrentPrice;
            if (price != null)
                wallet.Balance -= (double) price * coinAmount;

            await _purchasedCoinsRepository.SaveAsync();

            return new GenericDomainModel<PurchasedCoinDomainModel>
            {
                IsSuccessful = true,
                Data = _mapper.Map<PurchasedCoinDomainModel>(checkPurchasedCoin)
            };
        }

        public async Task<GenericDomainModel<PurchasedCoinDomainModel>> SellCoinAsync(Guid walletId, string coinId, double coinAmount)
        {
            var wallet = await _walletRepository.GetByIdAsync(walletId);
            if (wallet == null)
            {
                return new GenericDomainModel<PurchasedCoinDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.WALLET_ID_NULL
                };
            }

            var purchasedCoin= await _purchasedCoinsRepository.GetPurchasedCoinAsync(walletId, coinId);
            if (purchasedCoin == null)
            {
                return new GenericDomainModel<PurchasedCoinDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.PURCHASED_COIN_CANT_BE_SOLD
                };
            }

            var coinCurrantData = await _coinGeckoClient.CoinsClient.GetCoinMarkets("eur", new[] { coinId }, "market_cap_desc", 3, 1, false, "", "");
            if (coinCurrantData == null)
            {
                return new GenericDomainModel<PurchasedCoinDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.COINGECKO_COIN_DATA_ERROR
                };
            }

            if (purchasedCoin.Amount < coinAmount)
            {
                return new GenericDomainModel<PurchasedCoinDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.PURCHASED_COIN_NOT_ENOUGHT_COINS_IN_WALLET
                };
            }

            purchasedCoin.Amount -= coinAmount;

            if (coinCurrantData[0].CurrentPrice != null)
                wallet.Balance += coinAmount * (double) coinCurrantData[0].CurrentPrice;

            if (purchasedCoin.Amount == 0)
            {
                purchasedCoin = _purchasedCoinsRepository.DeletePurchasedCoin(purchasedCoin);
            }
            await _purchasedCoinsRepository.SaveAsync();

            return new GenericDomainModel<PurchasedCoinDomainModel>
            {
                IsSuccessful = true,
                Data = _mapper.Map<PurchasedCoinDomainModel>(purchasedCoin)
            };
        }

        public async Task<GenericDomainModel<PurchasedCoinDomainModel>> GetPurchasesByUserId(Guid userId)
        {
            var user =await _usersRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return new GenericDomainModel<PurchasedCoinDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.USER_ID_ERROR
                };
            }

            var purchases =await _purchasedCoinsRepository.GetPurchasedCoinsByUserIdAsync(userId);
            if (purchases == null)
            {
                return new GenericDomainModel<PurchasedCoinDomainModel>
                {
                    IsSuccessful = true,
                };
            }

            return new GenericDomainModel<PurchasedCoinDomainModel>
            {
                IsSuccessful = true,
                DataList = _mapper.Map<IEnumerable<PurchasedCoin>, List<PurchasedCoinDomainModel>>(purchases)
            };

        }

        public async Task<GenericDomainModel<PurchasedCoinDomainModel>> GetPurchase(Guid walletId, string coinId)
        {
            var wallet = await _walletRepository.GetByIdAsync(walletId);
            if (wallet == null)
            {
                return new GenericDomainModel<PurchasedCoinDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.WALLET_ID_NULL
                };
            }

            var purchase =await _purchasedCoinsRepository.GetPurchasedCoinAsync(walletId, coinId);
            if (purchase == null)
            {
                return new GenericDomainModel<PurchasedCoinDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.PURCHASED_CANT_BE_FOUND
                };
            }

            return new GenericDomainModel<PurchasedCoinDomainModel>
            {
                IsSuccessful = true,
                Data = _mapper.Map<PurchasedCoinDomainModel>(purchase)
            };

        }
    }
}