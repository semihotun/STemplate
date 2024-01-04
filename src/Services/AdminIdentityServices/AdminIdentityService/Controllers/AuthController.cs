using AdminIdentityService.Application.Handlers.AdminUsers.Commands.RegisterUser;
using AdminIdentityService.Application.Handlers.AdminUsers.Queries.LoginUsers;
using AdminIdentityService.Domain.Result;
using AdminIdentityService.Insfrastructure.Utilities.Security.Jwt;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace AdminIdentityService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IMediator mediator): ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        /// <summary>
        /// Admin Login oeperation
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Consumes("application/json")]
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(DataResult<AccessToken>))]
        [ProducesResponseType(StatusCodes.Status401Unauthorized, Type = typeof(string))]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] GetLoginUserQuery loginModel)
        {
            var result = await _mediator.Send(loginModel);
            if (result.Success)
            {
                return Ok(result);
            }
            return Unauthorized(result.Message);
        }
        /// <summary>
        ///  Make it User Register operations
        /// </summary>
        /// <param name="createUser"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Consumes("application/json")]
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IResult))]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(IResult))]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterUserCommand createUser)
        {
            var result = await _mediator.Send(createUser);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }
    }
}
