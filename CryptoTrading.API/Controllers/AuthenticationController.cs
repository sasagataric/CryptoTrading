
using CryptoTrading.API.Models;
using CryptoTrading.Domain.Interfaces;
using CryptoTrading.Domain.Models;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using static Google.Apis.Auth.GoogleJsonWebSignature;

namespace CryptoTrading.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly IUserService _userService;

        public AuthenticationController(IConfiguration config, IUserService userService)
        {
            _config = config;
            _userService = userService;
        }


        [HttpPost]
        [Route("login")]
        public async Task<ActionResult> Login([FromBody] AccessTokenModel accessToken)
        {
            Payload payload;
            try
            {
                payload = await ValidateAsync(accessToken.accessToken, new ValidationSettings
                {
                    Audience = new[] { _config.GetValue<string>("GoogleAuthSettings:clientId") }
                });

                var existingUser = await _userService.GetByUserEmailAsync(payload.Email);

                if (!existingUser.IsSuccessful)
                {
                    UserDomainModel newUser = new UserDomainModel
                    {
                        Email = payload.Email,
                        FirstName = payload.GivenName,
                        LastName = payload.FamilyName,
                        UserName = payload.Name,
                        Role = "user",
                        ProfilePicture = payload.Picture
                    };

                    var createdUser = await _userService.CreateUserAsync(newUser);

                    return Ok(createdUser.Data);
                }

                return Ok(existingUser.Data);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
