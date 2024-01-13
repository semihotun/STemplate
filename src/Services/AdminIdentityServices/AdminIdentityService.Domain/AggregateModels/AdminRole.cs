using AdminIdentityService.Domain.SeedWork;
namespace AdminIdentityService.Domain.AggregateModels
{
    public class AdminRole(string role) : BaseEntity
    {
        public string Role { get; } = role;
        public ICollection<AdminUserRole> AdminUserRoles { get; } = new List<AdminUserRole>();
    }
}
