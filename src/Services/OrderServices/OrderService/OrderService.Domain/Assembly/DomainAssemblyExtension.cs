using System.Reflection;

namespace OrderService.Domain.Assemblies
{
    public static class DomainAssemblyExtension
    {
        public static Assembly GetDomainAssembly()
        {
            return Assembly.GetExecutingAssembly();
        }
    }
}
