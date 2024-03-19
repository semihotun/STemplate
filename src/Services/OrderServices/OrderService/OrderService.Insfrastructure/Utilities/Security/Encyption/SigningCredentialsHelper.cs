using Microsoft.IdentityModel.Tokens;

namespace OrderService.Insfrastructure.Utilities.Security.Encyption
{
    public static class SigningCredentialsHelper
    {
        public static SigningCredentials CreateSigningCredentials(SecurityKey securityKey)
        {
            return new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
        }
    }
}
