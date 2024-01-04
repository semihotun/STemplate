using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
namespace DDDTemplateService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : ControllerBase
    {
        private IMediator _mediator;
        private readonly ILogger<MailController> _logger;
        private readonly IBus _bus;
        public MailController(ILogger<MailController> logger, IMediator mediator, IBus bus)
        {
            _logger = logger;
            _mediator = mediator;
            _bus = bus;
        }
        [Consumes("application/json")]
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [HttpGet("getmail")]
        public async Task<IActionResult> Index()
        {
            Console.Write("asdasdas");
            return Ok("İşlem tamam");
        }
        [Consumes("application/json")]
        [Produces("application/json", "text/plain")]
        [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(string))]
        [HttpPost("postmail")]
        public IActionResult Create(int id)
        {
            _logger.LogInformation("Yanlis Anlasilma" + id);
            return Ok("işlem tamam");
        }
    }
}
