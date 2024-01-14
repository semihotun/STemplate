using AdminIdentityService.Domain.SeedWork;
namespace AdminIdentityService.Domain.AggregateModels
{
    public class AdminRole : BaseEntity
    {
        public AdminRole(string role)
        {
            Role = role;
        }
        public string Role { get; private set; }
        public ICollection<AdminUserRole> AdminUserRoles { get; private set; } = [];
    }
}
