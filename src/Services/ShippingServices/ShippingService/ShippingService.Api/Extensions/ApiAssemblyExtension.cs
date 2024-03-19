using ShippingService.Application.Assemblies;
using ShippingService.Domain.Assemblies;
using ShippingService.Insfrastructure.Utilities.Assemblies;
using System.Reflection;

namespace ShippingService.Extensions
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
