using AdminIdentityService.Domain.SeedWork;
namespace AdminIdentityService.Domain.AggregateModels
{
    public class AdminRole(string role) : BaseEntity
    {
        public string Role { get; private set; } = role;
        public ICollection<AdminUserRole> AdminUserRoles { get; private set; } = new List<AdminUserRole>();
    }
}
