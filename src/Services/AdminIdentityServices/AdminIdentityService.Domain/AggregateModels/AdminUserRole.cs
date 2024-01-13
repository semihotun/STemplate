using AdminIdentityService.Domain.SeedWork;
namespace AdminIdentityService.Domain.AggregateModels
{
    public class AdminUserRole(Guid adminUserId, Guid adminRoleId) : BaseEntity
    {
        public Guid AdminUserId { get; init; } = adminUserId;
        public AdminUser? AdminUser { get; init; }
        public Guid AdminRoleId { get; init; } = adminRoleId;
        public AdminRole? AdminRole { get; init; }
    }
}
