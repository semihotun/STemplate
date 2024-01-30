using System.Reflection;
namespace STemplate.Domain.Assemblies
{
    public static class DomainAssemblyExtension
    {
        public static Assembly GetDomainAssembly()
        {
            return Assembly.GetExecutingAssembly();
        }
    }
}
