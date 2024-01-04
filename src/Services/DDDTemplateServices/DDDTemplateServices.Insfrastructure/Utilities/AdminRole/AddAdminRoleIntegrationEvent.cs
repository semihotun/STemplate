using DDDTemplateServices.Insfrastructure.Utilities.ServiceBus;
namespace DDDTemplateServices.Insfrastructure.Utilities.AdminRole
{
    [BusType<AddAdminRoleIntegrationEvent>]
    public class AddAdminRoleIntegrationEvent(string[] roleName) : IMessage
    {
        public string[] RoleName { get; set; } = roleName;
    }
}
