using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CryptoTrading.Data.Entities;
using CryptoTrading.Domain.Common;
using CryptoTrading.Domain.Interfaces;
using CryptoTrading.Domain.Models;
using CryptoTrading.Repositories.Interfaces;

namespace CryptoTrading.Domain.Services
{
    public class WalletHistoryService : IWalletHistoryService
    {
        private readonly IWalletHistoryRepository _walletHistoryRepository;
        private readonly IWalletsRepository _walletsRepositor ;
        private readonly ICoinsRepository _coinsRepository;
        private readonly IMapper _mapper;
        public WalletHistoryService(
            IWalletHistoryRepository walletHistoryRepository, 
            IMapper mapper, 
            IWalletsRepository walletsRepositor,
            ICoinsRepository coinsRepository)
        {
            _walletHistoryRepository = walletHistoryRepository;
            _mapper = mapper;
            _walletsRepositor = walletsRepositor;
            _coinsRepository = coinsRepository; 
        }

        public async Task<GenericDomainModel<WalletHistoryModel>> GetAllAsync()
        {
            var historys =await _walletHistoryRepository.GetAllAsync();
            if (historys == null)
            {
                return null;
            }

            return new GenericDomainModel<WalletHistoryModel>
            {
                DataList = _mapper.Map<IEnumerable<WalletHistory>, List<WalletHistoryModel>>(historys)
            };
        }

        public async Task<GenericDomainModel<WalletHistoryModel>> GetByWalletIdAsync(Guid walletId)
        {
            var wallet =await _walletsRepositor.GetByIdAsync(walletId);
            if (wallet == null)
            {
                return new GenericDomainModel<WalletHistoryModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.WALLET_ID_NULL
                };
            }

            var historys = await _walletHistoryRepository.GetByWalletIdAsync(walletId);
            if (historys == null)
            {
                return null;
            }

            return new GenericDomainModel<WalletHistoryModel>
            {
                IsSuccessful = true,
                DataList = _mapper.Map<IEnumerable<WalletHistory>, List<WalletHistoryModel>>(historys)
            };
        }

        public async Task<GenericDomainModel<WalletHistoryModel>> GetByWalletIdForCoinIdAsync(Guid walletId, string coinId)
        {
            var wallet = await _walletsRepositor.GetByIdAsync(walletId);
            if (wallet == null)
            {
                return new GenericDomainModel<WalletHistoryModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.WALLET_ID_NULL
                };
            }

            var coin = await _coinsRepository.GetByIdAsync(coinId);
            if (coin == null)
            {
                return new GenericDomainModel<WalletHistoryModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.COIN_ID_NULL
                };
            }

            var historys = await _walletHistoryRepository.GetByWalletIdForCoinIdAsync(walletId,coinId);
            if (historys == null)
            {
                return null;
            }

            return new GenericDomainModel<WalletHistoryModel>
            {
                IsSuccessful = true,
                DataList = _mapper.Map<IEnumerable<WalletHistory>, List<WalletHistoryModel>>(historys)
            };
        }

        public async Task<GenericDomainModel<WalletHistoryModel>> GetByWalletIdInRangeAsync(Guid walletId, DateTime startDate, DateTime endDate)
        {
            var wallet = await _walletsRepositor.GetByIdAsync(walletId);
            if (wallet == null)
            {
                return new GenericDomainModel<WalletHistoryModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.WALLET_ID_NULL
                };
            }

            var historys = await _walletHistoryRepository.GetByWalletIdInRangeAsync(walletId,startDate,endDate);
            if (historys == null)
            {
                return null;
            }

            return new GenericDomainModel<WalletHistoryModel>
            {
                IsSuccessful = true,
                DataList = _mapper.Map<IEnumerable<WalletHistory>, List<WalletHistoryModel>>(historys)
            };
        }
    }
}
