using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoTrading.API.Models;
using CryptoTrading.Domain.Interfaces;
using CryptoTrading.Repositories;
using Microsoft.AspNetCore.Authorization;

namespace CryptoTrading.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WalletHistoryController : ControllerBase
    {
        private readonly IWalletHistoryService _walletHistoryService;

        public WalletHistoryController(IWalletHistoryService walletHistoryService)
        {
            _walletHistoryService = walletHistoryService;
        }
        [Authorize(Policy = "Admin")]
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var historys =await _walletHistoryService.GetAllAsync();
            return Ok(historys.DataList);
        }

        [HttpGet]
        [Route("{walletId:Guid}")]
        public async Task<ActionResult> GetByWalletId(Guid walletId)
        {
            var history =await _walletHistoryService.GetByWalletIdAsync(walletId);

            if (!history.IsSuccessful)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = history.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            return Ok(history.DataList);
        }

        [HttpGet]
        [Route("{walletId:Guid}/coin/{coinId}")]
        public async Task<ActionResult> GetByWalletIdForCoinId(Guid walletId, string coinId)
        {
            var history = await _walletHistoryService.GetByWalletIdForCoinIdAsync(walletId, coinId);

            if (!history.IsSuccessful)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = history.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            return Ok(history.DataList);
        }

        [HttpGet]
        [Route("{walletId:Guid}/date-range/{start}/{end}")]
        public async Task<ActionResult> GetHistoryInRange(Guid walletId, DateTime start , DateTime end)
        {
            var history = await _walletHistoryService.GetByWalletIdInRangeAsync(walletId, start, end);

            if (!history.IsSuccessful)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = history.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            return Ok(history.DataList);
        }

    }
}
