using IdentityServer4.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IDServer
{
    public class CustomTokenRequestValidator : ICustomTokenRequestValidator
    {
        public Task ValidateAsync(CustomTokenRequestValidationContext context)
        {
            if (context.Result.ValidatedRequest.GrantType == "client_credentials") {
                context.Result.ValidatedRequest.AccessTokenLifetime = 60;
            }
            return Task.CompletedTask;
        }
    }
}
