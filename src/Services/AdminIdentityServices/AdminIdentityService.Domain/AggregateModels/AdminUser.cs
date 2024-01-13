using AdminIdentityService.Domain.SeedWork;
namespace AdminIdentityService.Domain.AggregateModels
{
    public class AdminUser(string firstName,
        string lastName,
        string email,
        byte[] passwordSalt,
        byte[] passwordHash,
        bool status) : BaseEntity, IAggregateRoot
    {
        public string FirstName { get; } = firstName;
        public string LastName { get; } = lastName;
        public string Email { get; } = email;
        public byte[] PasswordSalt { get; } = passwordSalt;
        public byte[] PasswordHash { get; } = passwordHash;
        public bool Status { get; } = status;
        public ICollection<AdminUserRole> AdminUserRoles { get; } = new List<AdminUserRole>();
        public void AddAdminUserRole(Guid roleId)
        {
            var orderItem = new AdminUserRole(base.Id, roleId);
            AdminUserRoles.Add(orderItem);
        }
    }
}
