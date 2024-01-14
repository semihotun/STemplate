using AdminIdentityService.Domain.SeedWork;
namespace AdminIdentityService.Domain.AggregateModels
{
    public class AdminUser : BaseEntity, IAggregateRoot
    {
        public AdminUser(string firstName,
        string lastName,
        string email,
        byte[] passwordSalt,
        byte[] passwordHash,
        bool status)
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PasswordSalt = passwordSalt;
            PasswordHash = passwordHash;
            Status = status;
        }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public byte[] PasswordSalt { get; private set; }
        public byte[] PasswordHash { get; private set; }
        public bool Status { get; private set; }
        public ICollection<AdminUserRole> AdminUserRoles { get; private set; } = [];
        public void AddAdminUserRole(Guid roleId)
        {
            var orderItem = new AdminUserRole(base.Id, roleId);
            AdminUserRoles.Add(orderItem);
        }
    }
}
