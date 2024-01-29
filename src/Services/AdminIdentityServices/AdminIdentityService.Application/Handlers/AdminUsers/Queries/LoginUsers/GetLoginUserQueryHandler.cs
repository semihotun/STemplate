using AdminIdentityService.Application.Constants;
using AdminIdentityService.Application.Handlers.AdminUsers.Queries.GetUserDtos;
using AdminIdentityService.Domain.Result;
using AdminIdentityService.Insfrastructure.Utilities.Caching.Redis;
using AdminIdentityService.Insfrastructure.Utilities.Security.Hashing;
using AdminIdentityService.Insfrastructure.Utilities.Security.Jwt;
using MediatR;
namespace AdminIdentityService.Application.Handlers.AdminUsers.Queries.LoginUsers;

public record GetLoginUserQuery(string Email, string Password) : IRequest<DataResult<AccessToken>>;

public class GetLoginUserQueryHandler(ITokenService tokenHelper,
    IMediator mediator,
    ICacheService cacheService) : IRequestHandler<GetLoginUserQuery, DataResult<AccessToken>>
{
    private readonly ITokenService _tokenHelper = tokenHelper;
    private readonly IMediator _mediator = mediator;
    private readonly ICacheService _cacheService = cacheService;

    public async Task<DataResult<AccessToken>> Handle(GetLoginUserQuery request, CancellationToken cancellationToken)
    {
        return await _cacheService.GetAsync<DataResult<AccessToken>>(request,
          async () =>
          {
              var user = await _mediator.Send(new GetUserDtoQuery(request.Email));
              if (!user.Success) return new ErrorDataResult<AccessToken>(Messages.UserNotFound);
              if (!HashingHelper.VerifyPasswordHash(request.Password, user.Data!.PasswordSalt, user.Data.PasswordHash))
              {
                  return new ErrorDataResult<AccessToken>(Messages.PasswordError);
              }
              var accessToken = _tokenHelper.CreateToken<AccessToken>(user.Data);
              if (user.Data.AdminUserRoles is not null)
              {
                  accessToken.Claims = user.Data.AdminUserRoles.Select(x => x.Role).ToList();
              }
              return new SuccessDataResult<AccessToken>(accessToken);
          }, cancellationToken);
    }
}