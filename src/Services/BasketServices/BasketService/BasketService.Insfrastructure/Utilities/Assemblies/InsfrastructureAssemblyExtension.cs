using System.Reflection;

namespace BasketService.Insfrastructure.Utilities.Assemblies
{
    public static class InsfrastructureAssemblyExtension
    {
        public static Assembly GetInsfrastructureAssembly()
        {
            return Assembly.GetExecutingAssembly();
        }
    }
}
