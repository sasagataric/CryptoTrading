using AutoMapper;
using CryptoTrading.API.Models;
using CryptoTrading.Data.Entities;
using CryptoTrading.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoTrading.API.Mapper
{
    public class ContollersProfileMapper : Profile
    {
        public ContollersProfileMapper()
        {
            CreateMap<CreateUserModel,UserDomainModel>().AfterMap((src, dest) => { dest.Role = "user"; 
                                                                                   dest.Id = Guid.NewGuid(); });

            CreateMap<CreateWalletModel, WalletDomainModel>().AfterMap((src, dest) => {dest.Id = Guid.NewGuid();  
                                                                                       dest.Profit = 0;});
        }
    }
}
