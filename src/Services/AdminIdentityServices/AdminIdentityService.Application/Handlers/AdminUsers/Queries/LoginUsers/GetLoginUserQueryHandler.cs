using AdminIdentityService.Application.Constants;
using AdminIdentityService.Application.Handlers.AdminUsers.Queries.GetUserDtos;
using AdminIdentityService.Domain.Result;
using AdminIdentityService.Insfrastructure.Utilities.Caching.Redis;
using AdminIdentityService.Insfrastructure.Utilities.Security.Hashing;
using AdminIdentityService.Insfrastructure.Utilities.Security.Jwt;
using MediatR;
namespace AdminIdentityService.Application.Handlers.AdminUsers.Queries.LoginUsers
{
    public record GetLoginUserQueryHandler : IRequestHandler<GetLoginUserQuery, DataResult<AccessToken>>
    {
        private readonly ITokenService _tokenHelper;
        private readonly IMediator _mediator;
        private readonly ICacheService _cacheService;
        public GetLoginUserQueryHandler(ITokenService tokenHelper, IMediator mediator, ICacheService cacheService)
        {
            _tokenHelper = tokenHelper;
            _mediator = mediator;
            _cacheService = cacheService;
        }
        public async Task<DataResult<AccessToken>> Handle(GetLoginUserQuery request, CancellationToken cancellationToken)
        {
            var data = await _cacheService.GetAsync<DataResult<AccessToken>>(request,
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
            return data;
        }
    }
}
