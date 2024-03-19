using System.Reflection;

namespace PaymentService.Application.Assemblies
{
    public static class ApplicationAssemblyExtension
    {
        public static Assembly GetApplicationAssembly()
        {
            return Assembly.GetExecutingAssembly();
        }
    }
}
