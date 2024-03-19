using CustomerService.Application.Assemblies;
using CustomerService.Domain.Assemblies;
using CustomerService.Insfrastructure.Utilities.Assemblies;
using System.Reflection;

namespace CustomerService.Extensions
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
