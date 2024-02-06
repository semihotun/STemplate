using AdminIdentityService.Application.Handlers.AdminUsers.Commands.RegisterUser;
using AdminIdentityService.Application.Handlers.AdminUsers.Queries.LoginUsers;
using AdminIdentityService.Domain.Result;
using AdminIdentityService.Insfrastructure.Utilities.Security.Jwt;
using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AdminIdentityService.Endpoints
{
    public class AuthEndpoints : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            var group = app.MapGroup("api/auth");

            group.MapPost("/login", Login)
                .Produces(StatusCodes.Status200OK, typeof(DataResult<AccessToken>))
                .AllowAnonymous()
                .WithOpenApi(operation => new(operation)
                {
                    Summary = "Login Page"
                });

            group.MapPost("/register", Register)
             .Produces(StatusCodes.Status200OK, typeof(Result))
             .AllowAnonymous()
             .WithOpenApi(operation => new(operation)
             {
                 Summary = "Register Page"
             });
        }
        public static async Task<IResult> Login([FromBody] GetLoginUserQuery loginModel, ISender sender)
        {
            var result = await sender.Send(loginModel);
            if (result.Success)
            {
                return Results.Ok(result);
            }
            return Results.BadRequest(result);
        }
        public static async Task<IResult> Register([FromBody] RegisterUserCommand createUser, ISender sender)
        {
            var result = await sender.Send(createUser);
            if (result.Success)
            {
                return Results.Ok(result);
            }
            return Results.BadRequest(result);
        }
    }
}
