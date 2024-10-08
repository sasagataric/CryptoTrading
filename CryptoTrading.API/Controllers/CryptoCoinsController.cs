﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

//FOR TESTING
namespace CryptoTrading.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize]
    public class CryptoCoinsController : ControllerBase
    {

        private readonly CoinGecko.Interfaces.ICoinGeckoClient _coinGeckoClient;

        public CryptoCoinsController(CoinGecko.Interfaces.ICoinGeckoClient coinGeckoClient)
        {
            _coinGeckoClient = coinGeckoClient;
            
        }


        [HttpGet]
        [Route("coin/GetAllCoinDataWithId/{id}")]
        public async Task<ActionResult> GetAllCoinDataWithId(string id)
        {
            //test
            var data = await _coinGeckoClient.CoinsClient.GetAllCoinDataWithId(id, "false", false,true,false,false,false);
            return Ok(data);

        }


        [HttpGet]
        [Route("coin/market_chart")]
        public async Task<ActionResult> GetCoinMarkets()
        {   //test
            string[] ids = { "bitcoin", "ethereum" };
            var data = await _coinGeckoClient.CoinsClient.GetCoinMarkets("usd",ids, "market_cap_desc",3,1, false, "","");
            return Ok(data);
        }
    }
}
