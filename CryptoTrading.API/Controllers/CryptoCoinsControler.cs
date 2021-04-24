using CoinGecko.Clients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading.Tasks;

namespace CryptoTrading.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CryptoCoinsControler : ControllerBase
    {

        private readonly CoinGecko.Interfaces.ICoinGeckoClient _coinGeckoClient;

        public CryptoCoinsControler(CoinGecko.Interfaces.ICoinGeckoClient coinGeckoClient)
        {
            _coinGeckoClient = coinGeckoClient;
            
        }


        [HttpGet]
        [Route("coin/GetAllCoinDataWithId/{id}")]
        public async Task<ActionResult> GetAllCoinDataWithId(string id)
        {
            var data = await _coinGeckoClient.CoinsClient.GetAllCoinDataWithId(id, "false", false,true,false,false,false);
            return Ok(data);

        }


        [HttpGet]
        [Route("coin/market_chart")]
        public async Task<ActionResult> GetCoinMarkets()
        {
            string[] ids = { "bitcoin", "ethereum" };
            var data = await _coinGeckoClient.CoinsClient.GetCoinMarkets("usd",ids, "market_cap_desc",3,1, false, "","");
            return Ok(data);
        }
    }
}
