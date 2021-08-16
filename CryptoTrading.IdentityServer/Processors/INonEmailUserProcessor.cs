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
    public class NonEmailUserProcessor<TUser> : INonEmailUserProcessor where TUser : User, new()
    {
        private readonly UserManager<TUser> _userManager;
        public NonEmailUserProcessor(
            UserManager<TUser> userManager
            )
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }
        public async Task<GrantValidationResult> ProcessAsync(JObject userInfo, string provider)
        {

            var userEmail = userInfo.Value<string>("email");

            if (provider.ToLower() == "linkedin")
                userEmail = userInfo.Value<string>("emailAddress");

            var userExternalId = userInfo.Value<string>("id");

            if (userEmail == null)
            {
                var existingUser = await _userManager.FindByLoginAsync(provider, userExternalId);
                if (existingUser == null)
                {
                    var customResponse = new Dictionary<string, object>();
                    customResponse.Add("userInfo", userInfo);
                    return new GrantValidationResult(TokenRequestErrors.InvalidRequest, "could not retrieve user's email from the given provider, include email paramater and send request again.", customResponse);

                }
                else
                {
                    existingUser = await _userManager.FindByIdAsync(existingUser.Id.ToString());
                    var userClaims = await _userManager.GetClaimsAsync(existingUser);
                    return new GrantValidationResult(existingUser.Id.ToString(), provider, userClaims, provider, null);
                }

            }
            else
            {
                var newUser = new TUser 
                    {   
                        Email = userEmail, 
                        UserName = userEmail.Substring(0, userEmail.LastIndexOf("@")), 
                        FirstName = userInfo.Value<string>("given_name"), 
                        LastName = userInfo.Value<string>("family_name"),
                        ProfilePicture = userInfo.Value<string>("picture")
                    };

                var result = await _userManager.CreateAsync(newUser);
                if (result.Succeeded)
                {
                    //await _userManager.AddClaimAsync(newUser, new Claim("role", "Admin"));
                    await _userManager.AddLoginAsync(newUser, new UserLoginInfo(provider, userExternalId, provider));
                    var userClaims = await _userManager.GetClaimsAsync(newUser);
                    return new GrantValidationResult(newUser.Id.ToString(), provider, userClaims, provider, null);
                }
                return new GrantValidationResult(TokenRequestErrors.InvalidRequest, "user could not be created, please try again");
            }

        }
    }
}