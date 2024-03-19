using System.Reflection;

namespace ShippingService.Application.Assemblies
{
    public static class ApplicationAssemblyExtension
    {
        public static Assembly GetApplicationAssembly()
        {
            return Assembly.GetExecutingAssembly();
        }
    }
}
