using System;
using CryptoTrading.API.Models;
using CryptoTrading.Domain.Interfaces;
using CryptoTrading.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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


        [HttpGet("{coinId}")]
        public async Task<ActionResult> GetById(string coinId)
        {
            var coin = await _coinService.GetByIdAsync(coinId);
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

        //[HttpPost("{coinId}")]
        //public async Task<ActionResult> CreateCoin(string coinId)
        //{
        //    GenericDomainModel<CoinDomainModel> coin;
        //    try
        //    {
        //        coin = await _coinService.CreateCoinAsync(coinId);
        //    }
        //    catch (DbUpdateException e)
        //    {
        //        ErrorResponseModel errorResponse = new ErrorResponseModel
        //        {
        //            ErrorMessage = e.InnerException.Message ?? e.Message,
        //            StatusCode = System.Net.HttpStatusCode.BadRequest
        //        };

        //        return BadRequest(errorResponse);
        //    }

        //    if (!coin.IsSuccessful)
        //    {
        //        ErrorResponseModel errorResponse = new ErrorResponseModel
        //        {
        //            ErrorMessage = coin.ErrorMessage,
        //            StatusCode = System.Net.HttpStatusCode.BadRequest
        //        };

        //        return BadRequest(errorResponse);
        //    }

        //    return CreatedAtAction(nameof(GetById), new { Id = coin.Data.Id }, coin.Data);
        //}

    }
}
