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
    public class PurchasedCoinsController : ControllerBase
    {
        private readonly IPurchasedCoinService _purchasedCoinService;

        public PurchasedCoinsController(IPurchasedCoinService purchasedCoinService)
        {
            _purchasedCoinService = purchasedCoinService;
        }
        // GET: api/<PurchasedCoinsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<PurchasedCoinsController>/5
        [HttpGet]
        [Route("GetPurchase")]
        public string GetPurchasedCoin(Guid walletId, string coinId)
        {
            return "value";
        }

        // POST api/<PurchasedCoinsController>
        [HttpPost]
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
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            if (!purchase.IsSuccessful)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = purchase.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            return CreatedAtAction(nameof(GetPurchasedCoin), new { wallet = purchase.Data.WalletId, coinId = purchase.Data.CoinId }, purchase.Data);
        }

        // PUT api/<PurchasedCoinsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<PurchasedCoinsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
