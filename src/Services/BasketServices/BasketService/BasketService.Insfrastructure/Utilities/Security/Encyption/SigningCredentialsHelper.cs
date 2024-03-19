﻿using Microsoft.IdentityModel.Tokens;

namespace BasketService.Insfrastructure.Utilities.Security.Encyption
{
    public static class SigningCredentialsHelper
    {
        public static SigningCredentials CreateSigningCredentials(SecurityKey securityKey)
        {
            return new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
        }
    }
}
