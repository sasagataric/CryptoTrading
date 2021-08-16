using CryptoTrading.Data.Entities;
using CryptoTrading.IdentityServer.Interfaces.Processors;
using IdentityServer4.Models;
using IdentityServer4.Validation;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CryptoTrading.IdentityServer.Processors
{
    public class EmailUserProcessor<TUser> : IEmailUserProcessor where TUser : User, new()
    {
        private readonly UserManager<TUser> _userManager;
        public EmailUserProcessor(
            UserManager<TUser> userManager
            )
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }
        public async Task<GrantValidationResult> ProcessAsync(JObject userInfo, string email, string provider)
        {
            var userEmail = email;
            var userExternalId = userInfo.Value<string>("id");

            if (string.IsNullOrWhiteSpace(userExternalId))
            {
                return new GrantValidationResult(TokenRequestErrors.InvalidRequest, "could not retrieve user Id from the token provided");
            }

            var existingUser = _userManager.FindByEmailAsync(userEmail).Result;
            if (existingUser != null)
            {
                return new GrantValidationResult(TokenRequestErrors.InvalidRequest, "User with specified email already exists");
            }

            var newUser = new TUser
            {
                Email = userEmail,
                UserName = userEmail,
                FirstName = userInfo.Value<string>("given_name"),
                LastName = userInfo.Value<string>("family_name"),
                ProfilePicture = userInfo.Value<string>("picture")
            }; 
            
            var result = _userManager.CreateAsync(newUser).Result;
            if (result.Succeeded)
            {
                //await _userManager.AddClaimAsync(newUser, new Claim("role", "Admin"));
                await _userManager.AddLoginAsync(newUser, new UserLoginInfo(provider, userExternalId, provider));
                var userClaims = _userManager.GetClaimsAsync(newUser).Result;
                return new GrantValidationResult(newUser.Id.ToString(), provider, userClaims, provider, null);
            }
            return new GrantValidationResult(TokenRequestErrors.InvalidRequest, "could not create user , please try again.");
        }
    }
}
