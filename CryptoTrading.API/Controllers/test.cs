using AutoMapper;
using CryptoTrading.Data.Entities;
using CryptoTrading.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CryptoTrading.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class test : ControllerBase
    {
        private readonly ICoinsRepository _coinsRepository;
        private readonly IMapper _mapper; 
        public test(ICoinsRepository coinsRepository,IMapper mapper)
        {
            _coinsRepository = coinsRepository;
            _mapper = mapper;
        }
        public class model
        {
            public string Id { get; set; }
            public string Name { get; set; }
            public string Image { get; set; }
            public string Symbol { get; set; }
        }

        public class MapperDomainModel : Profile
        {
            public MapperDomainModel()
            {
                CreateMap<Coin, model>();
            }
        }
        // GET: api/<test>
        [HttpGet]
        public async Task<List<model>> Get()
        {
            var coins = await _coinsRepository.GetAllAsync();
            var result = _mapper.Map<IEnumerable<Coin>, List<model>>(coins);
            return result;
        }
        [HttpGet]
        [Route("Watchlist/{id}")]
        public async Task<List<model>> GetWatchlist(Guid id)
        {
            var coins = await _coinsRepository.GetCoinsFromWathchlistByUserId(id);

            var result = _mapper.Map<IEnumerable<Coin>,List<model>>(coins);
            
            return result;
        }

 

    }
}
