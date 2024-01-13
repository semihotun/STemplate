using AdminIdentityService.Domain.SeedWork;
namespace AdminIdentityService.Domain.AggregateModels
{
    public class AdminUserRole(Guid adminUserId, Guid adminRoleId) : BaseEntity
    {
        public Guid AdminUserId { get; set; } = adminUserId;
        public AdminUser? AdminUser { get; set; }
        public Guid AdminRoleId { get; set; } = adminRoleId;
        public AdminRole? AdminRole { get; set; }
    }
}
