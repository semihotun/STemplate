using DDDTemplateService.Application.Assemblies;
using DDDTemplateService.Domain.Assemblies;
using DDDTemplateServices.Insfrastructure.Utilities.Assemblies;
using System.Reflection;
namespace DDDTemplateService.Extensions
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
