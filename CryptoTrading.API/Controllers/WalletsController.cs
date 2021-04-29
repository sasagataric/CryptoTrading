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
        // GET: api/<WalletsController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<WalletsController>/5
        [HttpGet("{id}")]
        public string GetById(int id)
        {
            return "value";
        }

        // POST api/<WalletsController>
        [HttpPost]
        public async Task<ActionResult> Post(CreateWalletModel model)
        { 
            GenericDomainModel<WalletDomainModel> createdWallet;
            try
            {
                createdWallet = await _walletService.CreateWallet(_mapper.Map<WalletDomainModel>(model));
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

        // PUT api/<WalletsController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<WalletsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
