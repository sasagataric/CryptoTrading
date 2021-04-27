using CryptoTrading.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTrading.Domain.Interfaces
{
    public interface IUserService
    {
        Task<GenericDomainModel<UserDomainModel>> GetAllAsync();
        Task<GenericDomainModel<UserDomainModel>> GetByIdAsync(Guid userId);
        Task<GenericDomainModel<UserDomainModel>> CreateUserAsync(UserDomainModel userToCreate);
        Task<GenericDomainModel<UserDomainModel>> GetByUserNameAsync(string username);
        Task<GenericDomainModel<UserDomainModel>> DeleteUserAsync(Guid userId);
        Task<GenericDomainModel<UserDomainModel>> UpdateUserAsync(Guid userId, UserDomainModel userToUpdate);
    }
}
