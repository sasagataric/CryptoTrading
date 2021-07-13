using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using CryptoTrading.Data.Entities;
using CryptoTrading.Domain.Common;
using CryptoTrading.Domain.Interfaces;
using CryptoTrading.Domain.Models;
using CryptoTrading.Repositories;

namespace CryptoTrading.Domain.Services
{
    public class WalletHistoryService : IWalletHistoryService
    {
        private readonly IWalletHistoryRepository _walletHistoryRepository;
        private readonly IWalletsRepositor _walletsRepositor ;
        private readonly IMapper _mapper;
        public WalletHistoryService(IWalletHistoryRepository walletHistoryRepository, IMapper mapper, IWalletsRepositor walletsRepositor)
        {
            _walletHistoryRepository = walletHistoryRepository;
            _mapper = mapper;
            _walletsRepositor = walletsRepositor;
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

            var historys = await _walletHistoryRepository.GetByWalletId(walletId);
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
