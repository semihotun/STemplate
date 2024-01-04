namespace AdminIdentityService.Insfrastructure.Utilities.Security.Jwt
{
    public record AccessToken : IAccessToken
    {
        public List<string> Claims { get; set; }
        public string Token { get; set; }
        public DateTime Expiration { get; set; }
    }
}
