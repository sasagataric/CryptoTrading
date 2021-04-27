using CryptoTrading.API.Models;
using CryptoTrading.Domain.Interfaces;
using CryptoTrading.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace CryptoTrading.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoinsController : ControllerBase
    {
        private readonly ICoinService _coinService;

        public CoinsController(ICoinService coinService)
        {
            _coinService = coinService;
        }


        // GET: api/<CoinsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<CoinsController>/5
        [HttpGet("getCoinById/{id}")]
        public async Task<ActionResult> GetById(string id)
        {
            var coin = await _coinService.GetByIdAsync(id);
            if (!coin.IsSuccessful)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = coin.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }
            return Ok(coin.Data);
        }

        // POST api/<CoinsController>
        [HttpPost]
        public async Task<ActionResult> CreateCoin(string coinID)
        {
            GenericDomainModel<CoinDomainModel> coin;
            try
            {
                coin = await _coinService.CreateCoinAsync(coinID);
            }
            catch (DbUpdateException e)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = e.InnerException.Message ?? e.Message,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            if (!coin.IsSuccessful)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = coin.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            return CreatedAtAction(nameof(GetById), new { Id = coin.Data.Id }, coin.Data);
        }

        // PUT api/<CoinsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CoinsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
