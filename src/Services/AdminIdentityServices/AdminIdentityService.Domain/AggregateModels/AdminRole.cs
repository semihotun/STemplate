using AdminIdentityService.Domain.SeedWork;
namespace AdminIdentityService.Domain.AggregateModels
{
    public class AdminRole(string role) : BaseEntity
    {
        public string Role { get; init; } = role;
        public ICollection<AdminUserRole> AdminUserRoles { get; init; } = [];
    }
}
