using BasketService.Insfrastructure.Utilities.ServiceBus;

namespace BasketService.Insfrastructure.Utilities.AdminRole
{
    [UrnType<AddAdminRoleIntegrationEvent>]
    public class AddAdminRoleIntegrationEvent(string[] roleName) : IMessage
    {
        public string[] RoleName { get; set; } = roleName;
    }
}
