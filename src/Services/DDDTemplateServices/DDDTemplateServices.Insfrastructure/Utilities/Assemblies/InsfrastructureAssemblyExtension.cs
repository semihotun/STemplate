using System.Reflection;
namespace DDDTemplateServices.Insfrastructure.Utilities.Assemblies
{
    public static class InsfrastructureAssemblyExtension
    {
        public static Assembly GetInsfrastructureAssembly()
        {
            return Assembly.GetExecutingAssembly();
        }
    }
}
