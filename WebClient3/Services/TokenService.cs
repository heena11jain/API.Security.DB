using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebClient3.Services
{
    public class TokenService : ITokenService
    {
        private DiscoveryDocumentResponse _discDocument { get; set; }

        // In this constructor we use the HTTPClient to get the Document data from the IdentityServer OpenID Configuration endpoint. Note that we are hardcoding the URLs here.Ideally, we will have to define them in appsettings.json and use IOptions pattern to retrieve them at runtime.
        public TokenService()
        {
            using (var client = new HttpClient())
            {
                _discDocument = client.GetDiscoveryDocumentAsync("https://localhost:5001/.well-known/openid-configuration").Result;
            }
        }

        public async Task<TokenResponse> GetToken(string scope)
        {// request token
            using (var client = new HttpClient())
            {
                var tokenResponse = await client.RequestPasswordTokenAsync(new PasswordTokenRequest
                {
                    Address = _discDocument.TokenEndpoint,
                    ClientId = "client3",
                    Scope = scope,
                    ClientSecret = "secret",
                });
                if (tokenResponse.IsError)
                {
                    throw new Exception("Token Error");
                }
                return tokenResponse;
            }
        }
    }
}
