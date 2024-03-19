using System.Reflection;

namespace BasketService.Application.Assemblies
{
    public static class ApplicationAssemblyExtension
    {
        public static Assembly GetApplicationAssembly()
        {
            return Assembly.GetExecutingAssembly();
        }
    }
}
