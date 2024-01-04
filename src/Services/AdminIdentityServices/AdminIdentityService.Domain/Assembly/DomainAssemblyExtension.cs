using System.Reflection;
namespace AdminIdentityService.Domain.Assemblies
{
    public static class DomainAssemblyExtension
    {
        public static Assembly GetDomainAssembly()
        {
            return Assembly.GetExecutingAssembly();
        }
    }
}
