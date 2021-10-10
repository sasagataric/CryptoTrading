using AutoMapper;
using CryptoTrading.Data.Entities;
using CryptoTrading.Domain.Common;
using CryptoTrading.Domain.Interfaces;
using CryptoTrading.Domain.Models;
using CryptoTrading.Repositories.Interfaces;
using System;
using System.Threading.Tasks;

namespace CryptoTrading.Domain.Services
{
    public class WalletService : IWalletService
    {
        private readonly IWalletsRepository _walletRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly IMapper _mapper;

        public WalletService(IWalletsRepository walletRepository, 
                            IUsersRepository usersRepository,               
                            IMapper mapper)
        {
            _walletRepository = walletRepository;
            _usersRepository = usersRepository;
            _mapper = mapper;
        }

        public async Task<GenericDomainModel<WalletDomainModel>> AddBalance(Guid walletId, decimal amount)
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

            wallet.Balance += amount;

            await _walletRepository.SaveAsync();

            return new GenericDomainModel<WalletDomainModel>
            {
                IsSuccessful = true,
                Data = _mapper.Map<WalletDomainModel>(wallet)
            };
        }

        public async Task<GenericDomainModel<WalletDomainModel>> CreateWalletAsync(WalletDomainModel newWallet)
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

            var checkUserWallet = await _walletRepository.GetByUserIdAsync(newWallet.UserId);
            if (checkUserWallet != null)
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

        public async Task<GenericDomainModel<WalletDomainModel>> GetByIdAsync(Guid walletId)
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

        public async Task<GenericDomainModel<WalletDomainModel>> GetWalletByUserIdAsync(Guid userId)
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

        public async Task<GenericDomainModel<WalletDomainModel>> UpdateWalletBalanceAsync(Guid userId, decimal balanceChange)
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
