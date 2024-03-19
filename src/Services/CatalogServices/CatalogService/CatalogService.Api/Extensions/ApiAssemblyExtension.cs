using CatalogService.Application.Assemblies;
using CatalogService.Domain.Assemblies;
using CatalogService.Insfrastructure.Utilities.Assemblies;
using System.Reflection;

namespace CatalogService.Extensions
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
