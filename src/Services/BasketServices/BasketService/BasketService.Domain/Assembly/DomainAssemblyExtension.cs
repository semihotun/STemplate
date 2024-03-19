using System.Reflection;

namespace BasketService.Domain.Assemblies
{
    public static class DomainAssemblyExtension
    {
        public static Assembly GetDomainAssembly()
        {
            return Assembly.GetExecutingAssembly();
        }
    }
}
