using AutoMapper;
using CryptoTrading.Data.Entities;
using CryptoTrading.Domain.Common;
using CryptoTrading.Domain.Interfaces;
using CryptoTrading.Domain.Models;
using CryptoTrading.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
                            IUsersRepository usersRepository,
                            IPurchasedCoinsRepository purchasedCoinsRepository,
                            ICoinsRepository coinsRepository,
                            CoinGecko.Interfaces.ICoinGeckoClient coinGeckoClient,
                            IMapper mapper)
        {
            _walletRepository = walletRepository;
            _usersRepository = usersRepository;
            _purchasedCoinsRepository = purchasedCoinsRepository;
            _coinsRepository = coinsRepository;
            _coinGeckoClient = coinGeckoClient;
            _mapper = mapper;
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

            var coinCurrantData = await _coinGeckoClient.CoinsClient.GetCoinMarkets("usd", new string[] { coinId }, "market_cap_desc", 3, 1, false, "", "");
            if (coinCurrantData == null)
            {
                return new GenericDomainModel<PurchasedCoinDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.COINGECKO_COIN_DATA_ERROR
                };
            }
            if (wallet.Balance < (double)coinCurrantData[0].CurrentPrice * coinAmount)
            {
                return new GenericDomainModel<PurchasedCoinDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.WALLET_INSUFFICIENT_FOUNDS
                };
            }

            var checkPurchasedCoin = await _purchasedCoinsRepository.GetPurchasedCoin(walletId, coinId);
            if (checkPurchasedCoin != null)
            {
                checkPurchasedCoin.Amount += coinAmount;
                await _purchasedCoinsRepository.SaveAsync();
            }
            else
            {
                var newPurchase = new PurchasedCoin
                {
                    Wallet = wallet,
                    WalletId = walletId,
                    Coin = coin == null ? _mapper.Map<Coin>(coinCurrantData[0]) : coin,
                    CoinId = coinId,
                    Amount = coinAmount
                };

                checkPurchasedCoin = await _purchasedCoinsRepository.Insert(newPurchase);
                if (checkPurchasedCoin == null)
                {
                    return new GenericDomainModel<PurchasedCoinDomainModel>
                    {
                        IsSuccessful = false,
                        ErrorMessage = Messages.PURCHASEDCOIN_CREATION_ERROR
                    };
                }
                await _purchasedCoinsRepository.SaveAsync();
            }

            wallet.Balance -= (double)coinCurrantData[0].CurrentPrice * coinAmount;

            await _purchasedCoinsRepository.SaveAsync();

            return new GenericDomainModel<PurchasedCoinDomainModel>
            {
                IsSuccessful = true,
                Data = _mapper.Map<PurchasedCoinDomainModel>(checkPurchasedCoin)
            };
        }
    }
}
