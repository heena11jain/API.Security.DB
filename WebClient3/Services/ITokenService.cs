using IdentityModel.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebClient3.Services
{
    public interface ITokenService
    {
        Task<TokenResponse> GetToken(string scope); //input parameter will be a string of scope content.
    }
}
