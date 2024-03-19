using ProductService.Insfrastructure.Utilities.ServiceBus;

namespace ProductService.Insfrastructure.Utilities.AdminRole
{
    [UrnType<AddAdminRoleIntegrationEvent>]
    public class AddAdminRoleIntegrationEvent(string[] roleName) : IMessage
    {
        public string[] RoleName { get; set; } = roleName;
    }
}
