using AutoMapper;
using CryptoTrading.Data.Entities;
using CryptoTrading.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoTrading.Domain.Common
{
    public class ServicesProfileMapper : Profile
    {
        public ServicesProfileMapper()
        {
            CreateMap<User, UserDomainModel>().ReverseMap();
        }
    }
}
