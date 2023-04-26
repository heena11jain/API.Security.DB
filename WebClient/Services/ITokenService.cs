using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebClient.Services
{
    // This service will talk to IDServer4 and request for an access token with which the MVC Project can access the API data
    public interface ITokenService
    {
        Task<TokenResponse> GetToken(string scope); //input parameter will be a string of scope content.
    }
}
