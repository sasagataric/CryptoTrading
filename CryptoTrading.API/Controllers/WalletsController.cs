using AutoMapper;
using CryptoTrading.API.Models;
using CryptoTrading.Domain.Interfaces;
using CryptoTrading.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CryptoTrading.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletsController : ControllerBase
    {
        private readonly IWalletService _walletService;
        private readonly IMapper _mapper;

        public WalletsController(IWalletService walletService , IMapper mapper)
        {
            _walletService = walletService;
            _mapper = mapper;
        }
        [HttpGet]
        [Route("getById/{id:Guid}")]
        public async Task<ActionResult> GetById(Guid id)
        {
            var wallet = await _walletService.GetByIdAsync(id);

            if (!wallet.IsSuccessful)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = wallet.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            return Ok(wallet.Data);
        }

        [HttpGet]
        [Route("getByUserId/{id:Guid}")]
        public async Task<ActionResult> GetByUserId(Guid id)
        {
            var wallet =await _walletService.GetWalletByUserIdAsync(id);

            if (!wallet.IsSuccessful)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = wallet.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            return Ok(wallet.Data);
        }

        // POST api/<WalletsController>
        [HttpPost]
        public async Task<ActionResult> CreateWallet(CreateWalletModel model)
        { 
            GenericDomainModel<WalletDomainModel> createdWallet;
            try
            {
                createdWallet = await _walletService.CreateWalletAsync(_mapper.Map<WalletDomainModel>(model));
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

            if (!createdWallet.IsSuccessful)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = createdWallet.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            return CreatedAtAction(nameof(GetById), new { Id = createdWallet.Data.Id }, createdWallet.Data);
        }

    }
}
