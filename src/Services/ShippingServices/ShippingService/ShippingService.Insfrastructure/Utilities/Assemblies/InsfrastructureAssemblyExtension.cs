using System.Reflection;

namespace ShippingService.Insfrastructure.Utilities.Assemblies
{
    public static class InsfrastructureAssemblyExtension
    {
        public static Assembly GetInsfrastructureAssembly()
        {
            return Assembly.GetExecutingAssembly();
        }
    }
}
