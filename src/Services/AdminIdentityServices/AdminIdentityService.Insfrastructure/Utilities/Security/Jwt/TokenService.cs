using AdminIdentityService.Insfrastructure.Utilities.Identity.Claims;
using AdminIdentityService.Insfrastructure.Utilities.Security.Encyption;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
namespace AdminIdentityService.Insfrastructure.Utilities.Security.Jwt
{
    public class TokenService : ITokenService
    {
        private readonly TokenOptions _tokenOptions;
        private DateTime _tokenExpiration;
        public IConfiguration _configuration { get; }
        public TokenService(IConfiguration configuration)
        {
            _configuration = configuration;
            _tokenOptions = _configuration.GetSection("TokenOptions").Get<TokenOptions>() ?? new TokenOptions();
            _tokenExpiration = DateTime.Now.AddMinutes(_tokenOptions.AccessTokenExpiration);
        }
        public string DecodeToken(string input)
        {
            var handler = new JwtSecurityTokenHandler();
            if (input.StartsWith("Bearer "))
            {
                input = input.Substring("Bearer ".Length);
            }
            return handler.ReadJwtToken(input).ToString();
        }
        public TAccessToken CreateToken<TAccessToken>(GetUserDto user)
            where TAccessToken : IAccessToken, new()
        {
            var securityKey = SecurityKeyHelper.CreateSecurityKey(_tokenOptions.SecurityKey);
            var signingCredentials = SigningCredentialsHelper.CreateSigningCredentials(securityKey);
            var jwt = CreateJwtSecurityToken(_tokenOptions, user, signingCredentials);
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var token = jwtSecurityTokenHandler.WriteToken(jwt);
            return new TAccessToken()
            {
                Token = token,
                Expiration = _tokenExpiration,
            };
        }
        public JwtSecurityToken CreateJwtSecurityToken(
            TokenOptions tokenOptions,
            GetUserDto user,
            SigningCredentials signingCredentials)
        {
            try
            {
                var jwts = new JwtSecurityToken(
                      tokenOptions.Issuer,
                      tokenOptions.Audience,
                      expires: _tokenExpiration,
                      notBefore: DateTime.Now,
                      claims: SetClaims(user),
                      signingCredentials: signingCredentials);
                return jwts;
            }
            catch (Exception ex)
            {
                throw;
            }
            var jwt = new JwtSecurityToken(
                tokenOptions.Issuer,
                tokenOptions.Audience,
                expires: _tokenExpiration,
                notBefore: DateTime.Now,
                claims: SetClaims(user),
                signingCredentials: signingCredentials);
            return jwt;
        }
        private IEnumerable<Claim> SetClaims(GetUserDto user)
        {
            var claims = new List<Claim>();
            claims.AddNameIdentifier(user.Id.ToString());
            claims.AddName($"{user.FirstName} {user.LastName}");
            claims.AddRoles(user.AdminUserRoles.Select(x => x.Role).ToArray());
            return claims;
        }
    }
}