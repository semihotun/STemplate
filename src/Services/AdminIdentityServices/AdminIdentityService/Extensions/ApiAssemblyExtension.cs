using AdminIdentityService.Application.Assemblies;
using AdminIdentityService.Domain.Assemblies;
using AdminIdentityService.Insfrastructure.Utilities.Assemblies;
using System.Reflection;
namespace AdminIdentityService.Extensions
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
