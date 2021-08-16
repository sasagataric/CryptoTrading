using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptoTrading.IdentityServer.Helpers
{
    public static class QueryBuilder
    {
        public static string GetQuery(Dictionary<string, string> values, ProviderType provider)
        {
            switch (provider)
            {
                case ProviderType.Facebook:

                    try
                    {
                        var fields = values["fields"];
                        var access_token = values["access_token"];
                        return $"?fields={fields}&access_token={access_token}";
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                case ProviderType.Google:

                    var google_access_token = values["token"];
                    return $"?access_token={google_access_token}";

                default:
                    return null;
            }
        }
    }
}
