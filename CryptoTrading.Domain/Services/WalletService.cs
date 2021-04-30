using AutoMapper;
using CryptoTrading.Data.Entities;
using CryptoTrading.Domain.Common;
using CryptoTrading.Domain.Interfaces;
using CryptoTrading.Domain.Models;
using CryptoTrading.Repositories;
using System;
using System.Threading.Tasks;

namespace CryptoTrading.Domain.Services
{
    public class WalletService : IWalletService
    {
        private readonly IWalletsRepositor _walletRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly IPurchasedCoinsRepository _purchasedCoinsRepository;
        private readonly ICoinsRepository _coinsRepository;
        private readonly CoinGecko.Interfaces.ICoinGeckoClient _coinGeckoClient;
        private readonly IMapper _mapper;

        public WalletService(IWalletsRepositor walletRepository, 
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

        public async Task<GenericDomainModel<WalletDomainModel>> CreateWallet(WalletDomainModel newWallet)
        {
            var checkUserId = await _usersRepository.GetByIdAsync(newWallet.UserId);
            if (checkUserId == null)
            {
                return new GenericDomainModel<WalletDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.USER_ID_ERROR
                };
            }

            var CheckUserWallet = await _walletRepository.GetByUserIdAsync(newWallet.UserId);
            if (CheckUserWallet != null)
            {
                return new GenericDomainModel<WalletDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.USER_HAVE_WALLET_ERROR
                };
            }

            var wallet = _mapper.Map<Wallet>(newWallet);

            var createdWallet = await _walletRepository.InsertAsync(wallet);
            if (createdWallet == null)
            {
                return new GenericDomainModel<WalletDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.WALLET_CREATION_ERROR
                };
            }
            await _walletRepository.SaveAsync();

            return new GenericDomainModel<WalletDomainModel>
            {
                IsSuccessful = true,
                Data = _mapper.Map<WalletDomainModel>(createdWallet)
            };
        }

        public async Task<GenericDomainModel<WalletDomainModel>> GetById(Guid walletId)
        {
            var wallet = await _walletRepository.GetByIdAsync(walletId);
            if (wallet == null)
            {
                return new GenericDomainModel<WalletDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.WALLET_ID_NULL
                };
            }
            return new GenericDomainModel<WalletDomainModel>
            {
                IsSuccessful = true,
                Data = _mapper.Map<WalletDomainModel>(wallet)
            };
        }

        public async Task<GenericDomainModel<WalletDomainModel>> GetWalletByUserId(Guid userId)
        {
            var checkUserId = await _usersRepository.GetByIdAsync(userId);
            if (checkUserId == null)
            {
                return new GenericDomainModel<WalletDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.USER_ID_ERROR
                };
            }

            var wallet = await _walletRepository.GetByUserIdAsync(userId);
            if (wallet == null)
            {
                return new GenericDomainModel<WalletDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.USER_DOES_NOT_HAVE_WALLET_ERROR
                };
            }

            return new GenericDomainModel<WalletDomainModel>
            {
                IsSuccessful = true,
                Data = _mapper.Map<WalletDomainModel>(wallet)
            };
        }

        public async Task<GenericDomainModel<WalletDomainModel>> UpdateWalletBalanceAsync(Guid userId, double balanceChange)
        {
            var checkUserId = await _usersRepository.GetByIdAsync(userId);
            if (checkUserId == null)
            {
                return new GenericDomainModel<WalletDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.USER_ID_ERROR
                };
            }

            var checkWallet = await _walletRepository.GetByUserIdAsync(userId);
            if (checkWallet == null)
            {
                return new GenericDomainModel<WalletDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.USER_DOES_NOT_HAVE_WALLET_ERROR
                };
            }

            if(checkWallet.Balance < balanceChange)
            {
                return new GenericDomainModel<WalletDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.WALLET_NOT_ENOUGHT_MONEY_ERROR
                };
            }

            checkWallet.Balance += balanceChange;
            await _walletRepository.SaveAsync();

            return new GenericDomainModel<WalletDomainModel>
            {
                IsSuccessful = true,
                Data = _mapper.Map<WalletDomainModel>(checkWallet)
            };
        }

    }
}
