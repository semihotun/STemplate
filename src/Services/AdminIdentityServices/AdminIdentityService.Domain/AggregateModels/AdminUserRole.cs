using AdminIdentityService.Domain.SeedWork;
namespace AdminIdentityService.Domain.AggregateModels
{
    public class AdminUserRole : BaseEntity
    {
        public Guid AdminUserId { get; private set; }
        public AdminUser? AdminUser { get; private set; }
        public Guid AdminRoleId { get; private set; }
        public AdminRole? AdminRole { get; private set; }
        public AdminUserRole(Guid adminUserId,Guid adminRoleId)
        {
            AdminUserId = adminUserId;
            AdminRoleId = adminRoleId;
        }
        public void SetAdminUser(AdminUser? adminUser)
        {
            AdminUser = adminUser;
        }
        public void SetAdminRole(AdminRole? adminRole)
        {
            AdminRole = adminRole;
        }
    }
}
