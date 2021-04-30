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
        private readonly IMapper _mapper;
        private readonly CoinGecko.Interfaces.ICoinGeckoClient _coinGeckoClient;
        public CoinService(ICoinsRepository coinsRepository,IMapper mapper, CoinGecko.Interfaces.ICoinGeckoClient coinGeckoClient)
        {
            _coinsRepository = coinsRepository;
            _mapper = mapper;
            _coinGeckoClient = coinGeckoClient;
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

        public async Task<GenericDomainModel<CoinDomainModel>> DeleteVoinAsync(string coinId)
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
