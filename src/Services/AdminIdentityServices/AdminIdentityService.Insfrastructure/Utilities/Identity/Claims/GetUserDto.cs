namespace AdminIdentityService.Insfrastructure.Utilities.Identity.Claims
{
    public class GetUserDto
    {
        public class AdminUserRole
        {
            public Guid AdminUserRoleId { get; set; }
            public required string Role { get; set; }
        }
        public Guid Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public required string Email { get; set; }
        public required byte[] PasswordSalt { get; set; }
        public required byte[] PasswordHash { get; set; }
        public bool Status { get; set; }
        public IEnumerable<AdminUserRole>? AdminUserRoles { get; set; } = new List<AdminUserRole>();
    }
}
