using SearchBarService.Application.Assemblies;
using SearchBarService.Domain.Assemblies;
using SearchBarService.Insfrastructure.Utilities.Assemblies;
using System.Reflection;

namespace SearchBarService.Extensions
{
    public static class ApiAssemblyExtensions
    {
        public static Assembly[] GetLibrariesAssemblies()
        {
            var application = ApplicationAssemblyExtension.GetApplicationAssembly();
            var domain = DomainAssemblyExtension.GetDomainAssembly();
            var insfrastructure = InsfrastructureAssemblyExtension.GetInsfrastructureAssembly();
            return [application, domain, insfrastructure];
        }
    }
}
