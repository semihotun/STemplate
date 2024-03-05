using AdminIdentityService.Application.Constants;
using AdminIdentityService.Domain.AggregateModels;
using AdminIdentityService.Domain.Result;
using AdminIdentityService.Insfrastructure.Utilities.Caching.Redis;
using AdminIdentityService.Insfrastructure.Utilities.Security.Hashing;
using AdminIdentityService.Persistence.GenericRepository;
using AdminIdentityService.Persistence.UnitOfWork;
using MediatR;
namespace AdminIdentityService.Application.Handlers.AdminUsers.Commands.RegisterUser;

public record RegisterUserCommand(string Email, string Password, string FirstName, string LastName) : IRequest<Result>;

public class CreateRegisterUserCommandHandler(IRepository<AdminUser> adminUserRepository,
    IUnitOfWork unitOfWork,
    ICacheService cacheService) : IRequestHandler<RegisterUserCommand, Result>
{
    private readonly IRepository<AdminUser> _adminUserRepository = adminUserRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICacheService _cacheService = cacheService;

    public async Task<Result> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        return await _unitOfWork.BeginTransaction<Result>(async () =>
        {
            if (await _adminUserRepository.AnyAsync(u => u.Email == request.Email))
            {
                return new ErrorResult(Messages.NameAlreadyExist);
            }
            HashingHelper.CreatePasswordHash(request.Password, out var passwordSalt, out var passwordHash);
            var user = new AdminUser(firstName: request.FirstName,
                                     lastName: request.LastName,
                                     email: request.Email,
                                     passwordSalt: passwordSalt,
                                     passwordHash: passwordHash,
                                     status: true);
            await _adminUserRepository.AddAsync(user);
            await _cacheService.RemovePatternAsync("AdminIdentityService:AdminUsers");
            return new SuccessResult(Messages.Added);
        });
    }
}