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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CryptoTrading.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public UsersController(IUserService userService, IMapper mapper)
        {
            _userService = userService;
            _mapper = mapper;
        }
        // GET: api/<UsersController>
        [HttpGet("GetAll")]
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

        // GET api/<UsersController>/5
        [HttpGet("GetByIdAsync/{userId:Guid}")]
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

        // POST api/<UsersController>
        [HttpPost("Create")]
        public async Task<ActionResult> CreateUserAsync(CreateUserModel createUser)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var newUser = _mapper.Map<UserDomainModel>(createUser);

            GenericDomainModel<UserDomainModel> createdUser;
            try
            {
                createdUser = await _userService.CreateUserAsync(newUser);
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

            if (!createdUser.IsSuccessful)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = createdUser.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            return CreatedAtAction(nameof(GetUserById), new { Id = createdUser.Data.Id }, createdUser.Data);
        }

        [HttpPut("Update/{userId:Guid}")]
        public async Task<ActionResult> UpdateUser(Guid userId, [FromBody] UpdateUserModel updateUser)
        {
            if (userId == Guid.Empty)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = Messages.USER_ID_REQUIRED,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };
                return BadRequest(errorResponse);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedUser = await _userService.UpdateUserAsync(userId, _mapper.Map<UserDomainModel>(updateUser));
            if (!updatedUser.IsSuccessful)
            {
                ErrorResponseModel errorResponse = new ErrorResponseModel
                {
                    ErrorMessage = updatedUser.ErrorMessage,
                    StatusCode = System.Net.HttpStatusCode.BadRequest
                };

                return BadRequest(errorResponse);
            }

            return Accepted(updatedUser.Data);
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("Delete/{userId}")]
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
