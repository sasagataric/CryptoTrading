using System;
using System.Collections.Generic;
using AutoMapper;
using CryptoTrading.Data.Entities;
using CryptoTrading.Domain.Common;
using CryptoTrading.Domain.Interfaces;
using CryptoTrading.Domain.Models;
using CryptoTrading.Repositories;
using System.Threading.Tasks;

namespace CryptoTrading.Domain.Services
{
    public class CoinService : ICoinService
    {
        private readonly ICoinsRepository _coinsRepository;
        private readonly IUsersRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly CoinGecko.Interfaces.ICoinGeckoClient _coinGeckoClient;
        public CoinService(ICoinsRepository coinsRepository,IMapper mapper, CoinGecko.Interfaces.ICoinGeckoClient coinGeckoClient, IUsersRepository userRepository)
        {
            _coinsRepository = coinsRepository;
            _mapper = mapper;
            _coinGeckoClient = coinGeckoClient;
            _userRepository = userRepository;
        }

        public async Task<GenericDomainModel<CoinDomainModel>> CreateCoinAsync(string coinId)
        {
            var checkCoin = await _coinsRepository.GetByIdAsync(coinId);
            if (checkCoin != null)
            {
                return new GenericDomainModel<CoinDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.COIN_ALREDY_EXISTS
                };
            }

            var coinFromCoinGecko = await _coinGeckoClient.CoinsClient.GetAllCoinDataWithId(coinId);
            var coin = _mapper.Map<Coin>(coinFromCoinGecko);


            var insertedCoin = await _coinsRepository.InsertAsync(coin);
            if (insertedCoin == null)
            {
                return new GenericDomainModel<CoinDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.COIN_CREATION_ERROR
                };
            }

            await _coinsRepository.SaveAsync();

            return new GenericDomainModel<CoinDomainModel>
            {
                IsSuccessful = true,
                Data = _mapper.Map<CoinDomainModel>(insertedCoin)
            };
        }

        public async Task<GenericDomainModel<CoinGecko.Entities.Response.Coins.CoinMarkets>> GetWatchListCoinsByUserIdAsync(Guid userId)
        {
            var checkUserId = await _userRepository.GetByIdAsync(userId);
            if (checkUserId == null)
            {
                return new GenericDomainModel<CoinGecko.Entities.Response.Coins.CoinMarkets>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.USERS_NOT_FOUND
                };
            }

            var coins =await _coinsRepository.GetCoinsFromWathchlistByUserId(userId);

            if (checkUserId == null)
            {
                return null;
            }

            var coinsIds = new List<string>();

            foreach (var coin in coins)
            {
                coinsIds.Add((coin.Id));
            }

            var coinsFormApi = await _coinGeckoClient.CoinsClient.GetCoinMarkets("eur", coinsIds.ToArray() , "market_cap_desc", null,null , false, "", "");

            if (coinsFormApi == null)
            {
                return new GenericDomainModel<CoinGecko.Entities.Response.Coins.CoinMarkets>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.COINGECKO_COIN_DATA_ERROR
                };
            }

            return new GenericDomainModel<CoinGecko.Entities.Response.Coins.CoinMarkets>
            {
                IsSuccessful = true,
                DataList = coinsFormApi
            };
        }

        public async Task<GenericDomainModel<CoinDomainModel>> addCoinToWatchListAsync(Guid userId, string coinId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return new GenericDomainModel<CoinDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.USERS_NOT_FOUND
                };
            }

            var checkCoin = await _coinsRepository.GetByIdAsync(coinId);

            if (checkCoin == null)
            {
                var coinsFormApi = await _coinGeckoClient.CoinsClient.GetCoinMarkets("eur", new string[] { coinId }, "market_cap_desc", null, null, false, "", "");

                if (coinsFormApi == null)
                {
                    return new GenericDomainModel<CoinDomainModel>
                    {
                        IsSuccessful = false,
                        ErrorMessage = Messages.COINGECKO_COIN_DATA_ERROR
                    };
                }

                checkCoin = _mapper.Map<Coin>(coinsFormApi[0]);

            }

            

            user.Coins.Add(checkCoin);

           await _coinsRepository.SaveAsync();

            return new GenericDomainModel<CoinDomainModel>
            {
                IsSuccessful = true,
            };
        }

        public async Task<GenericDomainModel<CoinDomainModel>> DeleteCoinAsync(string coinId)
        {
            var coin = await _coinsRepository.GetByIdAsync(coinId);
            if (coin == null)
            {
                return new GenericDomainModel<CoinDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.COIN_ID_NULL
                };
            }

            var deletedCoin = _coinsRepository.Delete(coin);
            if (deletedCoin == null)
            {
                return new GenericDomainModel<CoinDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.COIN_DELETE_ERROR
                };
            }
            await _coinsRepository.SaveAsync();

            return new GenericDomainModel<CoinDomainModel>
            {
                IsSuccessful = true,
                Data = _mapper.Map<CoinDomainModel>(deletedCoin)
            };
        }

        public async Task<GenericDomainModel<CoinDomainModel>> GetByIdAsync(string coinId)
        {
            var coin = await _coinsRepository.GetByIdAsync(coinId);
            if (coin == null)
            {
                return new GenericDomainModel<CoinDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.COIN_ID_NULL
                };
            }

            return new GenericDomainModel<CoinDomainModel>
            {
                IsSuccessful = true,
                Data = _mapper.Map<CoinDomainModel>(coin)
            };
        }
    }
}
