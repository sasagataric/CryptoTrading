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
    public class UserService : IUserService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IMapper _mapper;
        public UserService(IUsersRepository usersRepository, IMapper mapper)
        {
            _usersRepository = usersRepository;
            _mapper = mapper;
        }

        public async Task<GenericDomainModel<UserDomainModel>> CreateUserAsync(UserDomainModel userToCreate)
        {
            var usernameCheck = await _usersRepository.CheckUsername(userToCreate.UserName);
            var emailCheck = await _usersRepository.CheckEmail(userToCreate.Email);

            if (!emailCheck || !usernameCheck)
            {
                return new GenericDomainModel<UserDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = (!usernameCheck ? Messages.USER_CREATION_ERROR_USERNAME_EXISTS : "") + (!emailCheck ? Messages.USER_CREATION_ERROR_EMAIL_EXISTS : "")
                };
            }

            var newUser = _mapper.Map<User>(userToCreate);

            var insertedUser = await _usersRepository.InsertAsync(newUser);
            if(insertedUser == null)
            {
                return new GenericDomainModel<UserDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.USER_CREATION_ERROR
                };
            }

            await _usersRepository.SaveAsync();

            return new GenericDomainModel<UserDomainModel>
            {
                IsSuccessful = true,
                Data = _mapper.Map<UserDomainModel>(insertedUser)
            };
        }

        public async Task<GenericDomainModel<UserDomainModel>> DeleteUserAsync(Guid userId)
        {
            var checkUser = await _usersRepository.GetByIdAsync(userId);
            if (checkUser == null)
            {
                return new GenericDomainModel<UserDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.USER_CREATION_ERROR
                };
            }

            var deletedUser = _usersRepository.Delete(checkUser);
            return new GenericDomainModel<UserDomainModel>
            {
                IsSuccessful = true,
                Data = _mapper.Map<UserDomainModel>(deletedUser)
            };
        }

        public Task<GenericDomainModel<UserDomainModel>> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<GenericDomainModel<UserDomainModel>> GetByIdAsync(Guid userId)
        {
            var user = await _usersRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return new GenericDomainModel<UserDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.USER_ID_NULL
                };
            }

            return new GenericDomainModel<UserDomainModel>
            {
                IsSuccessful = true,
                Data = _mapper.Map<UserDomainModel>(user)
            };
        }

        public async Task<GenericDomainModel<UserDomainModel>> GetByUserNameAsync(string username)
        {
            var user = await _usersRepository.GetByUserNameAsync(username);
            if (user == null)
            {
                return new GenericDomainModel<UserDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.USER_ID_NULL
                };
            }

            return new GenericDomainModel<UserDomainModel>
            {
                IsSuccessful = true,
                Data = _mapper.Map<UserDomainModel>(user)
            };
        }

        public async Task<GenericDomainModel<UserDomainModel>> UpdateUserAsync(Guid userId, UserDomainModel userToUpdate)
        {
            var user = await _usersRepository.GetByIdAsync(userId);
            if (user == null)
            {
                return new GenericDomainModel<UserDomainModel>
                {
                    IsSuccessful = false,
                    ErrorMessage = Messages.USER_ID_NULL
                };
            }

            if (user.UserName != userToUpdate.UserName)
            {
                var userUserName = await _usersRepository.CheckUsername(userToUpdate.UserName);
                if (!userUserName)
                {
                    return new GenericDomainModel<UserDomainModel>
                    {
                        IsSuccessful = false,
                        ErrorMessage = Messages.USER_ID_NULL
                    };
                }
            }

            if (user.Email != userToUpdate.Email)
            {
                var userUserName = await _usersRepository.CheckEmail(userToUpdate.Email);
                if (userUserName)
                {
                    return new GenericDomainModel<UserDomainModel>
                    {
                        IsSuccessful = false,
                        ErrorMessage = Messages.USER_ID_NULL
                    };
                }
            }



            user.Email = userToUpdate.Email;
            user.FirstName = userToUpdate.FirstName;
            user.LastName = userToUpdate.LastName;
            user.UserName = userToUpdate.UserName;

            await _usersRepository.SaveAsync();

            return new GenericDomainModel<UserDomainModel>
            {
                IsSuccessful = true,
                Data = _mapper.Map<UserDomainModel>(user)
            };
        }
    }
}
