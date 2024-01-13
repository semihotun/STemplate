using AdminIdentityService.Domain.Result;
using AdminIdentityService.Insfrastructure.Utilities.Security.Jwt;
using MediatR;
namespace AdminIdentityService.Application.Handlers.AdminUsers.Queries.LoginUsers
{
    public record GetLoginUserQuery : IRequest<DataResult<AccessToken>>
    {
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}
