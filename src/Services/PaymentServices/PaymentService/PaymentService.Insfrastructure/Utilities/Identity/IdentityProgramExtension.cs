using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PaymentService.Insfrastructure.Utilities.Security.Encyption;
using PaymentService.Insfrastructure.Utilities.Security.Jwt;

namespace PaymentService.Insfrastructure.Utilities.Identity
{
    public static class IdentityProgramExtension
    {
        public static void AddIdentitySettings(this WebApplicationBuilder builder)
        {
            var tokenOptions = builder.Configuration.GetSection("TokenOptions").Get<TokenOptions>() ?? new TokenOptions();
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            SaveSigninToken = true,
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidIssuer = tokenOptions.Issuer,
                            ValidAudience = tokenOptions.Audience,
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = SecurityKeyHelper.CreateSecurityKey(tokenOptions.SecurityKey)
                        };
                    });
        }
    }
}
