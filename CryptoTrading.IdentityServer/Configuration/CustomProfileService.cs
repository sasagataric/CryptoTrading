using CryptoTrading.Data.Entities;
using IdentityServer4.Extensions;
using IdentityServer4.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoTrading.IdentityServer.Configuration
{
    public class CustomProfileService : IProfileService
    {
        private readonly IUserClaimsPrincipalFactory<User> _claimsFactory;
        private readonly UserManager<User> _userManager;

        public CustomProfileService(IUserClaimsPrincipalFactory<User> claimsFactory, UserManager<User> userManager)
        {
            _claimsFactory = claimsFactory;
            _userManager = userManager;
        }
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            if (user == null)
            {
                throw new ArgumentException("");
            }

            var principal = await _claimsFactory.CreateAsync(user);
            var claims = principal.Claims.ToList();

            claims.Add(new System.Security.Claims.Claim("picture", user.ProfilePicture));
            claims.Add(new System.Security.Claims.Claim("first_name", user.FirstName));
            claims.Add(new System.Security.Claims.Claim("last_name", user.LastName));

            context.IssuedClaims = claims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            context.IsActive = user != null;
        }
    }
}
