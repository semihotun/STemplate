using AdminIdentityService.Domain.SeedWork;
namespace AdminIdentityService.Domain.AggregateModels
{
    public class AdminUserRole : BaseEntity
    {
        public Guid AdminUserId { get; set; }
        public AdminUser? AdminUser { get; set; }
        public Guid AdminRoleId { get; set; }
        public AdminRole? AdminRole { get; set; }
        public AdminUserRole(Guid adminUserId, Guid adminRoleId)
        {
            AdminUserId = adminUserId;
            AdminRoleId = adminRoleId;
        }
    }
}
