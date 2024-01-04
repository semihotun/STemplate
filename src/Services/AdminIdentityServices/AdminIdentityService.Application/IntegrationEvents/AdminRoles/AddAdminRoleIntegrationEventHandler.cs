using AdminIdentityService.Application.Handlers.AdminRoles.Commands.AdminRoleRequests;
using AdminIdentityService.Insfrastructure.Utilities.AdminRole;
using MassTransit;
using MediatR;
namespace AdminIdentityService.Application.IntegrationEvents.AdminRoles
{
    public class AddAdminRoleIntegrationEventHandler(IMediator mediator) : IConsumer<AddAdminRoleIntegrationEvent>
    {
        private readonly IMediator _mediator = mediator;
        public async Task Consume(ConsumeContext<AddAdminRoleIntegrationEvent> context)
        {
            await _mediator.Send(new AdminRoleCommand(context.Message.RoleName));
        }
    }
}
