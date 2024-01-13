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
        public string FirstName { get; init; } = firstName;
        public string LastName { get; init; } = lastName;
        public string Email { get; init; } = email;
        public byte[] PasswordSalt { get; init; } = passwordSalt;
        public byte[] PasswordHash { get; init; } = passwordHash;
        public bool Status { get; init; } = status;
        public ICollection<AdminUserRole> AdminUserRoles { get; init; } = [];
        public void AddAdminUserRole(Guid roleId)
        {
            var orderItem = new AdminUserRole(base.Id, roleId);
            AdminUserRoles.Add(orderItem);
        }
    }
}
