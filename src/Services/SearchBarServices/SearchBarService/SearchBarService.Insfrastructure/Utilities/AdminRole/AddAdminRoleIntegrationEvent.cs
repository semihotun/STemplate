using SearchBarService.Insfrastructure.Utilities.ServiceBus;

namespace SearchBarService.Insfrastructure.Utilities.AdminRole
{
    [UrnType<AddAdminRoleIntegrationEvent>]
    public class AddAdminRoleIntegrationEvent(string[] roleName) : IMessage
    {
        public string[] RoleName { get; set; } = roleName;
    }
}
