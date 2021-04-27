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
        [HttpGet]
        public IEnumerable<string> GetUser()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<UsersController>/5
        [HttpGet("{id}")]
        public string GetUserById(int id)
        {
            return "value";
        }

        // POST api/<UsersController>
        [HttpPost]
        [Route("Create")]
        public async Task<ActionResult<GenericDomainModel<UserDomainModel>>> CreateUserAsync(CreateUserModel createUser)
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

        // PUT api/<UsersController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
