﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Net;
using System.Security.Claims;
namespace AdminIdentityService.Insfrastructure.Utilities.Identity.Middleware
{
    public class ClaimMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        private readonly string[] notIgnorePath = ["/metrics", "/healthcheck"];
        public static string UserNotLogged => "Kullanıcı Giriş yapmadı";
        private readonly RequestDelegate _next = next;
        private readonly IConfiguration _configuration = configuration;
        public async Task InvokeAsync(HttpContext httpContext)
        {
            var path = httpContext.Request.Path.Value;
            var isAllowAnonymous = httpContext.GetEndpoint()?.Metadata.GetMetadata<AllowAnonymousAttribute>();
            var endPoint = httpContext.GetEndpoint();
            if (!notIgnorePath.Any(x => x == path) && isAllowAnonymous is null && endPoint is not null)
            {
                //Check Login in
                if ((httpContext.User.Identity?.IsAuthenticated) == false)
                {
                    throw new UnauthorizedAccessException(UserNotLogged);
                }
                //Check Role
                var regionPath = path?.Replace("/api", _configuration["RegionName"]).ToLower();
                var userRoles = httpContext.User
                    .FindAll(ClaimTypes.Role)
                    .Any(x => x.Value == regionPath);
                if (!userRoles)
                {
                    throw new UnauthorizedAccessException(UserNotLogged);
                }
            }
            await _next(httpContext);
        }
    }
}
