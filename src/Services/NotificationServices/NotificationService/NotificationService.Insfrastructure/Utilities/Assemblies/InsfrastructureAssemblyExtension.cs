using System.Reflection;

namespace NotificationService.Insfrastructure.Utilities.Assemblies
{
    public static class InsfrastructureAssemblyExtension
    {
        public static Assembly GetInsfrastructureAssembly()
        {
            return Assembly.GetExecutingAssembly();
        }
    }
}
