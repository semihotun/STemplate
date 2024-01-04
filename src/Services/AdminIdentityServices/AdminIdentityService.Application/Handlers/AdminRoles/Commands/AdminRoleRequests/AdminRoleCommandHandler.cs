using AdminIdentityService.Domain.AggregateModels;
using AdminIdentityService.Domain.Result;
using AdminIdentityService.Insfrastructure.Utilities.Caching.Redis;
using AdminIdentityService.Persistence.Context;
using AdminIdentityService.Persistence.GenericRepository;
using MediatR;
namespace AdminIdentityService.Application.Handlers.AdminRoles.Commands.AdminRoleRequests
{
    public record AdminRoleCommand(string[] RolePath) : IRequest<Result>
    {
        public record AdminRoleCommandHandler : IRequestHandler<AdminRoleCommand, Result>
        {
            private readonly IRepository<AdminRole> _adminRoleRepository;
            private readonly ICoreDbContext _coreDbContext;
            private readonly ICacheService _cacheService;
            public AdminRoleCommandHandler(IRepository<AdminRole> adminRoleRepository, ICoreDbContext coreDbContext, ICacheService cacheService)
            {
                _adminRoleRepository = adminRoleRepository;
                _coreDbContext = coreDbContext;
                _cacheService = cacheService;
            }
            public async Task<Result> Handle(AdminRoleCommand request, CancellationToken cancellationToken)
            {
                return await _coreDbContext.BeginTransaction<Result>(async () =>
                {
                    var newProducts = request.RolePath
                               .Where(newAdminRole => !_adminRoleRepository.Any(existingAdminRole => existingAdminRole.Role == newAdminRole))
                               .Select(x => new AdminRole(x));
                    if (newProducts != null)
                    {
                        await _adminRoleRepository.AddRangeAsync(newProducts);
                    }
                    await _cacheService.RemovePatternAsync("AdminIdentityService:AdminRoles");
                    return new SuccessResult();
                });
            }
        }
    }
}
