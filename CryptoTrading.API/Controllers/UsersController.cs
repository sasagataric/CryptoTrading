using System;
using AutoMapper;
using CryptoTrading.API.Models;
using CryptoTrading.Domain.Interfaces;
using CryptoTrading.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using CryptoTrading.Domain.Common;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Newtonsoft.Json;

namespace CryptoTrading.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        private readonly ICoinService _coinService;

        public UsersController(IUserService userService, IMapper mapper, ICoinService coinService)
        {
            _userService = userService;
            _mapper = mapper;
            _coinService = coinService;
        }

        [Authorize(Policy ="Admin")]
        [HttpGet]
        public async Task<ActionResult> GetAllUser()
        {
            var users = await _userService.GetAllAsync();

            if (!users.IsSuccessful)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = users.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            return Ok(users.DataList);
        }
        [Authorize(Policy = "Admin")]
        [HttpGet("{userId:Guid}")]
        public async Task<ActionResult> GetUserById(Guid userId)
        {
            var user = await _userService.GetByIdAsync(userId);

            if (!user.IsSuccessful)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = user.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            return Ok(user.Data);
        }

        [HttpGet]
        [Route("{userId:Guid}/watch-list")]
        public async Task<ActionResult> GetWatchListByUserId(Guid userId)
        {
            GenericDomainModel<CoinGecko.Entities.Response.Coins.CoinMarkets> coins;
            try
            {
                coins = await _coinService.GetWatchListByUserIdAsync(userId);
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
           
            if (!coins.IsSuccessful)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = coins.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            string jsonData = JsonConvert.SerializeObject(coins.DataList, Formatting.Indented);

            return Ok(jsonData);

        }

        [HttpPost]
        [Route("watch-list/add")]
        public async Task<ActionResult> AddToWatchList([FromBody]WatchListModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            GenericDomainModel<CoinDomainModel> addedCoin;
            try
            {
                addedCoin = await _coinService.AddCoinToWatchListAsync(model.UserId, model.CoinId);
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

        [Route("watch-list/remove")]
        public async Task<ActionResult> RemoveFromWatchList([FromBody] WatchListModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            GenericDomainModel<CoinDomainModel> addedCoin;
            try
            {
                addedCoin = await _coinService.RemoveCoinFromWatchListAsync(model.UserId, model.CoinId);
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

        //[HttpPost("create")]
        //public async Task<ActionResult> CreateUserAsync(CreateUserModel createUser)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var newUser = _mapper.Map<UserDomainModel>(createUser);

        //    GenericDomainModel<UserDomainModel> createdUser;
        //    try
        //    {
        //        createdUser = await _userService.CreateUserAsync(newUser);
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

        //    if (!createdUser.IsSuccessful)
        //    {
        //        ErrorResponseModel errorResponse = new ErrorResponseModel
        //        {
        //            ErrorMessage = createdUser.ErrorMessage,
        //            StatusCode = System.Net.HttpStatusCode.BadRequest
        //        };

        //        return BadRequest(errorResponse);
        //    }

        //    return CreatedAtAction(nameof(GetUserById), new { Id = createdUser.Data.Id }, createdUser.Data);
        //}

        //[HttpPut("{userId:Guid}/update")]
        //public async Task<ActionResult> UpdateUser(Guid userId, [FromBody] UpdateUserModel updateUser)
        //{
        //    if (userId == Guid.Empty)
        //    {
        //        ErrorResponseModel errorResponse = new ErrorResponseModel
        //        {
        //            ErrorMessage = Messages.USER_ID_REQUIRED,
        //            StatusCode = System.Net.HttpStatusCode.BadRequest
        //        };
        //        return BadRequest(errorResponse);
        //    }

        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var updatedUser = await _userService.UpdateUserAsync(userId, _mapper.Map<UserDomainModel>(updateUser));
        //    if (!updatedUser.IsSuccessful)
        //    {
        //        ErrorResponseModel errorResponse = new ErrorResponseModel
        //        {
        //            ErrorMessage = updatedUser.ErrorMessage,
        //            StatusCode = System.Net.HttpStatusCode.BadRequest
        //        };

        //        return BadRequest(errorResponse);
        //    }

        //    return Accepted(updatedUser.Data);
        //}

        [Authorize(Policy = "Admin")]
        [HttpDelete("{userId:Guid}/delete")]
        public async Task<ActionResult> DeleteUser(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = Messages.USER_ID_ERROR,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
                return BadRequest(errorResponse);
            }

            var deleteUser = await _userService.DeleteUserAsync(userId);

            if (!deleteUser.IsSuccessful)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = deleteUser.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }
            return Accepted();
        }
    }
}
