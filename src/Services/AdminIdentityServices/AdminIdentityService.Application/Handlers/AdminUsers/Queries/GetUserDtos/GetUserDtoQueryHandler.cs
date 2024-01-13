using AdminIdentityService.Application.Constants;
using AdminIdentityService.Domain.AggregateModels;
using AdminIdentityService.Domain.Result;
using AdminIdentityService.Insfrastructure.Utilities.Identity.Claims;
using AdminIdentityService.Persistence.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;
namespace AdminIdentityService.Application.Handlers.AdminUsers.Queries.GetUserDtos
{
    public record GetUserDtoQueryHandler : IRequestHandler<GetUserDtoQuery, DataResult<GetUserDto>>
    {
        private readonly ICoreDbContext _context;
        public GetUserDtoQueryHandler(ICoreDbContext context)
        {
            _context = context;
        }
        public async Task<DataResult<GetUserDto>> Handle(GetUserDtoQuery request, CancellationToken cancellationToken)
        {
            var data = from us in _context.Query<AdminUser>()
                       where us.Status && us.Email == request.Email
                       let adminUserRoles = (from usr in _context.Query<AdminUserRole>().DefaultIfEmpty()
                                             where usr.AdminUserId == us.Id
                                             join ar in _context.Query<AdminRole>() on usr.AdminRoleId equals ar.Id
                                             select new GetUserDto.AdminUserRole
                                             {
                                                 AdminUserRoleId = usr.AdminUserId,
                                                 Role = ar.Role
                                             }).AsEnumerable()
                       select new GetUserDto
                       {
                           Id = us.Id,
                           FirstName = us.FirstName,
                           LastName = us.LastName,
                           Email = us.Email,
                           PasswordSalt = us.PasswordSalt,
                           PasswordHash = us.PasswordHash,
                           Status = us.Status,
                           AdminUserRoles = adminUserRoles,
                       };
            var result = await data.FirstOrDefaultAsync(cancellationToken);
            if (result == null)
            {
                return new ErrorDataResult<GetUserDto>(Messages.UserNotFound);
            }
            return new SuccessDataResult<GetUserDto>(result);
        }
    }
}
