using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using CryptoTrading.API.Models;
using CryptoTrading.Domain.Interfaces;
using CryptoTrading.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CryptoTrading.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchasedCoinsController : ControllerBase
    {
        private readonly IPurchasedCoinService _purchasedCoinService;

        public PurchasedCoinsController(IPurchasedCoinService purchasedCoinService)
        {
            _purchasedCoinService = purchasedCoinService;
        }

        // GET api/<PurchasedCoinsController>/5
        [HttpGet]
        [Route("GetPurchase")]
        public async Task<ActionResult> GetPurchase(Guid walletId, string coinId)
        {
            var purchase = await _purchasedCoinService.GetPurchase(walletId, coinId);

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
        [Route("GetPurchaseByUserId/{userId:Guid}")]
        public async Task<ActionResult> GetPurchasesByUserId(Guid userId)
        {
            var purchases = await _purchasedCoinService.GetPurchasesByUserId(userId);

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
        [HttpPost("BuyCoin")]
        public async Task<ActionResult> PurchaseCoin(PurchaseCoinModel purchaseModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            GenericDomainModel<PurchasedCoinDomainModel> purchase;
            try
            {
                purchase = await _purchasedCoinService.BuyCoinAsync(purchaseModel.WalletId, purchaseModel.CoinId, purchaseModel.Amount);
            }
            catch (DbUpdateException e)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = e.InnerException.Message ?? e.Message,
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

            return CreatedAtAction(nameof(GetPurchase), new { wallet = purchase.Data.WalletId, coinId = purchase.Data.Coin.Id }, purchase.Data);
        }

        [HttpPost("SellCoin")]
        public async Task<ActionResult> SellCoin(PurchaseCoinModel purchaseModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            GenericDomainModel<PurchasedCoinDomainModel> sellCoin;
            try
            {
                sellCoin = await _purchasedCoinService.SellCoinAsync(purchaseModel.WalletId, purchaseModel.CoinId, purchaseModel.Amount);
            }
            catch (DbUpdateException e)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = e.InnerException.Message ?? e.Message,
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

            return CreatedAtAction(nameof(GetPurchase), new { wallet = sellCoin.Data.WalletId, coinId = sellCoin.Data.Coin.Id }, sellCoin.Data);
        }

    }
}
