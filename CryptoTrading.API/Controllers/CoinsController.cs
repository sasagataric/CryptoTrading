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

        [HttpGet]
        [Route("watchList/byUserId/{id:Guid}")]
        public async Task<ActionResult> GetWatchListCoinsByUserId(Guid id)
        {
            var coins = await _coinService.GetWatchListCoinsByUserIdAsync(id);
            if (!coins.IsSuccessful)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = coins.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }
            return Ok(coins.DataList);
        }

        [HttpPost]
        [Route("watchList/addForUser")]
        public async Task<ActionResult> AddToWatchList(AddCoinToWatchListModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            GenericDomainModel<CoinDomainModel> addedCoin;
            try
            {
                addedCoin = await _coinService.addCoinToWatchListAsync(model.UserId,model.CoinId);
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

            if (!addedCoin.IsSuccessful)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = addedCoin.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            return Ok();
        }

        [HttpPost]
        public async Task<ActionResult> CreateCoin(string coinId)
        {
            GenericDomainModel<CoinDomainModel> coin;
            try
            {
                coin = await _coinService.CreateCoinAsync(coinId);
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

    }
}
