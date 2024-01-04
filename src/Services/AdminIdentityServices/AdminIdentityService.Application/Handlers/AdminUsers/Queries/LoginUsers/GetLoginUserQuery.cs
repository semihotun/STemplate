using AdminIdentityService.Domain.Result;
using AdminIdentityService.Insfrastructure.Utilities.Security.Jwt;
using MediatR;
namespace AdminIdentityService.Application.Handlers.AdminUsers.Queries.LoginUsers
{
    public record GetLoginUserQuery : IRequest<DataResult<AccessToken>>
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
