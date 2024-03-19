using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace NotificationService.Insfrastructure.Utilities.Identity.Middleware
{
    public class ClaimMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        private readonly string[] notIgnorePath = ["/metrics", "/healthcheck"];
        private readonly RequestDelegate _next = next;
        private readonly IConfiguration _configuration = configuration;
        public async Task InvokeAsync(HttpContext httpContext)
        {
            if (httpContext.GetEndpoint()?.Metadata.GetMetadata<AllowAnonymousAttribute>() == null &&
                !notIgnorePath.Any(x => x == httpContext.Request.Path.Value))
            {
                if ((httpContext.User.Identity?.IsAuthenticated) == false)
                {
                    throw new UnauthorizedAccessException("User Not Logged In");
                }
                if (!httpContext.User
                    .FindAll(ClaimTypes.Role)
                    .Any(x => x.Value == httpContext.Request.Path.Value?.Replace("/api", _configuration["RegionName"]).ToLower()))
                {
                    throw new UnauthorizedAccessException("Not Access");
                }
            }
            await _next(httpContext);
        }
    }
}
