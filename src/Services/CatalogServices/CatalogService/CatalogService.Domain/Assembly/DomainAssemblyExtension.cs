using System.Reflection;

namespace CatalogService.Domain.Assemblies
{
    public static class DomainAssemblyExtension
    {
        public static Assembly GetDomainAssembly()
        {
            return Assembly.GetExecutingAssembly();
        }
    }
}
