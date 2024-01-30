using STemplate.Application.Assemblies;
using STemplate.Domain.Assemblies;
using STemplate.Insfrastructure.Utilities.Assemblies;
using System.Reflection;
namespace STemplate.Extensions
{
    public static class ApiAssemblyExtensions
    {
        public static Assembly[] GetLibrariesAssemblies()
        {
            var application = ApplicationAssemblyExtension.GetApplicationAssembly();
            var domain = DomainAssemblyExtension.GetDomainAssembly();
            var insfrastructure = InsfrastructureAssemblyExtension.GetInsfrastructureAssembly();
            return new Assembly[]
            {
               application,domain,insfrastructure
            };
        }
    }
}
