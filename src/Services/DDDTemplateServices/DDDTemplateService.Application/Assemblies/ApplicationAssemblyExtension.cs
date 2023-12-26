using System.Reflection;
namespace DDDTemplateService.Application.Assemblies
{
    public static class ApplicationAssemblyExtension
    {
        public static Assembly GetApplicationAssembly()
        {
            return Assembly.GetExecutingAssembly();
        }
    }
}
