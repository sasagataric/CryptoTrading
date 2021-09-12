using IdentityModel;
using IdentityServer4.Models;
using IdentityServer4.Test;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using static IdentityServer4.IdentityServerConstants;

namespace CryptoTrading.IdentityServer.Configuration
{
    public class Config
    {
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityServer4.Models.IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
                new IdentityResources.Email(),
                new IdentityResource
                {
                    Name = "role",
                    DisplayName = "Role",
                    UserClaims = { JwtClaimTypes.Role }
                },
                new IdentityResource(
                name: "custom_profile_data",
                userClaims: new[] { "first_name", "last_name", "picture" },
                displayName: "Your profile data")

            };
        }
        public static IEnumerable<ApiScope> GetApiScopes()
        {
            return new List<ApiScope>
            {
                new ApiScope("WebApi.ExternalLogin")
            };
        }
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("WebApi")
                {
                    Scopes =  new List<string>{"WebApi.ExternalLogin"}
                }
            };
        }

      
        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
            {
                new Client
                {
                    ClientId="crypto-trading-web-app",
                    ClientName="Crypto Trading Web App",
                    ClientSecrets =
                    {
                        new Secret("Some secret".Sha256())
                    },
                    AllowedScopes = {
                        StandardScopes.Email,
                        StandardScopes.OpenId,
                        StandardScopes.Profile,
                        "profile",
                        "role",
                        "WebApi.ExternalLogin"
                    },
                    RequireConsent = false,
                    AllowedGrantTypes = new[] {GrantType.ResourceOwnerPassword,"external"},
                    AllowAccessTokensViaBrowser = true,
                    AccessTokenType = AccessTokenType.Jwt,
                    AlwaysIncludeUserClaimsInIdToken = true,
                    AccessTokenLifetime = 86400,
                    AllowOfflineAccess = true,
                    IdentityTokenLifetime = 86400,
                    AlwaysSendClientClaims = true,
                    Enabled = true,
                }
            };
        }

    }
}
