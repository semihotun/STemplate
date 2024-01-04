using AdminIdentityService.Domain.Result;
using AdminIdentityService.Insfrastructure.Utilities.Identity.Claims;
using MediatR;
namespace AdminIdentityService.Application.Handlers.AdminUsers.Queries.GetUserDtos
{
    public record GetUserDtoQuery : IRequest<DataResult<GetUserDto>>
    {
        public string Email { get; set; }
        public GetUserDtoQuery(string email)
        {
            Email = email;
        }
    }
}
