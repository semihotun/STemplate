using AdminIdentityService.Insfrastructure.Utilities.ServiceBus;
namespace AdminIdentityService.Insfrastructure.Utilities.AdminRole
{
    [BusType<AddAdminRoleIntegrationEvent>]
    public class AddAdminRoleIntegrationEvent(string[] roleName) : IMessage
    {
        public string[] RoleName { get; set; } = roleName;
    }
}
