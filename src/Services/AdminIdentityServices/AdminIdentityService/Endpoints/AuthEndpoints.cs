using AdminIdentityService.Application.Handlers.AdminUsers.Commands.RegisterUser;
using AdminIdentityService.Application.Handlers.AdminUsers.Queries.LoginUsers;
using AdminIdentityService.Domain.Result;
using AdminIdentityService.Insfrastructure.Utilities.Security.Jwt;
using Carter;
using Carter.OpenApi;
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
                .IncludeInOpenApi();

            group.MapPost("/register", Register)
             .Produces(StatusCodes.Status200OK, typeof(Result))
             .AllowAnonymous();

            group.MapPost("/deneme", Deneme)
            .Produces(StatusCodes.Status200OK, typeof(DataResult<AccessToken>));
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
        public static async Task<IResult> Deneme([FromBody] RegisterUserCommand createUser, ISender sender)
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
