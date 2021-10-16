using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using CryptoTrading.API.Models;
using CryptoTrading.Domain.Interfaces;
using CryptoTrading.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CryptoTrading.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class HoldingsController : ControllerBase
    {
        private readonly IHoldingService _holdingService;

        public HoldingsController(IHoldingService holdingService)
        {
            _holdingService = holdingService;
        }

        [HttpGet("{walletId:Guid}&{coinId}")]
        public async Task<ActionResult> GetHolding(Guid walletId, string coinId)
        {
            var purchase = await _holdingService.GetHolding(walletId, coinId);

            if (!purchase.IsSuccessful)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = purchase.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            return Ok(purchase.Data);
        }

        [HttpGet]
        [Route("{userId:Guid}")]
        public async Task<ActionResult> GetHoldingsByUserId(Guid userId)
        {
            var purchases = await _holdingService.GetHoldingsByUserId(userId);

            if (!purchases.IsSuccessful)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = purchases.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            return Ok(purchases.DataList);
        }

        // POST api/<PurchasedCoinsController>
        [HttpPost("buy")]
        public async Task<ActionResult> BuyCoin(HoldingModel purchaseModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            GenericDomainModel<HoldingDomainModel> purchase;
            try
            {
                purchase = await _holdingService.BuyCoinAsync(purchaseModel.WalletId, purchaseModel.CoinId, purchaseModel.Amount);
            }
            catch (DbUpdateException e)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = (e.InnerException != null) ? e.InnerException.Message : e.Message,
                    StatusCode = HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            if (!purchase.IsSuccessful)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = purchase.ErrorMessage,
                    StatusCode = HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }
            return CreatedAtAction(nameof(GetHolding), new { walletId = purchase.Data.WalletId, coinId = purchase.Data.Coin.Id }, purchase.Data);
        }

        [HttpPost("sell")]
        public async Task<ActionResult> SellCoin(HoldingModel sellModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            GenericDomainModel<HoldingDomainModel> sellCoin;
            try
            {
                sellCoin = await _holdingService.SellCoinAsync(sellModel.WalletId, sellModel.CoinId, sellModel.Amount);
            }
            catch (DbUpdateException e)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = (e.InnerException != null) ? e.InnerException.Message : e.Message,
                    StatusCode = HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            if (!sellCoin.IsSuccessful)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = sellCoin.ErrorMessage,
                    StatusCode = HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            return CreatedAtAction(nameof(GetHolding), new { walletId = sellCoin.Data.WalletId, coinId = sellCoin.Data.Coin.Id }, sellCoin.Data);
        }

    }
}
