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
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly IHttpClientFactory _clientFactory;

        private readonly CoinGecko.Interfaces.ICoinGeckoClient _coinGeckoClient;

        public CryptoCoinsControler( IHttpClientFactory clientFactory, CoinGecko.Interfaces.ICoinGeckoClient coinGeckoClient)
        {
            _coinGeckoClient = coinGeckoClient;
            _clientFactory = clientFactory;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }



        [HttpGet]
        [Route("coin/marketChart")]
        public async Task<ActionResult> GetCoinMatker()
        {
            var data = await _coinGeckoClient.CoinsClient.GetAllCoinDataWithId("bitcoin");
            return Ok(data);

            //var resourceUri = "https://api.coingecko.com/api/v3/coins/markets?vs_currency=usd&ids=bitcoin,ethereum&order=market_cap_desc&per_page=100&page=1&sparkline=false";
          
            //var client = _clientFactory.CreateClient();

            //var response = await client.SendAsync(new HttpRequestMessage(HttpMethod.Get, resourceUri));

            //if (response.IsSuccessStatusCode)
            //{
            //    var responseStream = await response.Content.ReadFromJsonAsync<IEnumerable<MarketData>>();
            //    return Ok(responseStream);
            //}

            //return BadRequest();
        }


        [HttpGet]
        [Route("coin/market_chart")]
        public async Task<ActionResult> GetCoinMarkets()
        {
            var data = await _coinGeckoClient.CoinsClient.GetCoinMarkets("usd");
            return Ok(data);
        }
    }
}
