namespace NotificationService.Insfrastructure.Utilities.Security.Jwt
{
    public class TokenOptions
    {
        public string Audience { get; set; } = "Secret Auidience";
        public string Issuer { get; set; } = "Secret Issuer";
        public int AccessTokenExpiration { get; set; } = 10;
        public string SecurityKey { get; set; } = "Secret SecurityKey";
    }
}
