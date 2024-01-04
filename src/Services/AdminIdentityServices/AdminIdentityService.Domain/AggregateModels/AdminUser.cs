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
        public string FirstName { get; private set; } = firstName;
        public string LastName { get; private set; } = lastName;
        public string Email { get; private set; } = email;
        public byte[] PasswordSalt { get; private set; } = passwordSalt;
        public byte[] PasswordHash { get; private set; } = passwordHash;
        public bool Status { get; private set; } = status;
        public ICollection<AdminUserRole> AdminUserRoles { get; private set; } = new List<AdminUserRole>();
        public void AddAdminUserRole(Guid roleId)
        {
            var orderItem = new AdminUserRole(base.Id, roleId);
            AdminUserRoles.Add(orderItem);
        }
    }
}
