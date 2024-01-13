namespace AdminIdentityService.Insfrastructure.Utilities.Security.Jwt
{
    public interface IAccessToken
    {
        DateTime Expiration { get; set; }
        string? Token { get; set; }
    }
}