using AdminIdentityService.Insfrastructure.Utilities.Identity.Claims;
namespace AdminIdentityService.Insfrastructure.Utilities.Security.Jwt
{
    public interface ITokenService
    {
        TAccessToken CreateToken<TAccessToken>(GetUserDto user)
          where TAccessToken : IAccessToken, new();
    }
}
