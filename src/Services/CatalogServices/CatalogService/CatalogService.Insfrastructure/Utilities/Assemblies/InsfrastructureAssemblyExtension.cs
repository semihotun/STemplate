using System.Reflection;

namespace CatalogService.Insfrastructure.Utilities.Assemblies
{
    public static class InsfrastructureAssemblyExtension
    {
        public static Assembly GetInsfrastructureAssembly()
        {
            return Assembly.GetExecutingAssembly();
        }
    }
}
