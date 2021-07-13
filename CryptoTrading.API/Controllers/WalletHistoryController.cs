using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoTrading.API.Models;
using CryptoTrading.Domain.Interfaces;
using CryptoTrading.Repositories;


namespace CryptoTrading.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletHistoryController : ControllerBase
    {
        private readonly IWalletHistoryService _walletHistoryService;

        public WalletHistoryController(IWalletHistoryService walletHistoryService)
        {
            _walletHistoryService = walletHistoryService;
        }
        // GET: api/<WalletHistoryController>
        [HttpGet]
        [Route("all")]
        public async Task<ActionResult> GetAll()
        {
            var historys =await _walletHistoryService.GetAllAsync();
            return Ok(historys.DataList);
        }

        // GET api/<WalletHistoryController>/5
        [HttpGet]
        [Route("getByWalletId/{id:Guid}")]
        public async Task<ActionResult> GetById(Guid id)
        {
            var history =await _walletHistoryService.GetByWalletIdAsync(id);

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
