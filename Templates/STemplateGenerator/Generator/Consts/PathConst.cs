using EnvDTE;
using Generator.Extensions;
using Generator.Models;
using System.IO;
namespace Generator.Const;

/// <summary>
/// Return path
/// </summary>
internal static class PathConst
{
    /// <summary>
    /// Get Handler Path
    /// </summary>
    /// <param name="aggregateClassPath"></param>
    /// <param name="aggregateClassName"></param>
    /// <returns></returns>
    public static string HandlerPath(string aggregateClassPath, string aggregateClassName)
    {
        return Path.Combine(Path.GetDirectoryName(aggregateClassPath)
            .Replace("Domain", "Application")
            .Replace("AggregateModels", "Handlers"),
            $"{aggregateClassName.Plurualize()}");
    }
    /// <summary>
    /// Get Project Name for Aggregate Class
    /// </summary>
    /// <param name="aggregateClassName"></param>
    /// <returns></returns>
    public static string GetProjectName(ProjectItem aggregateClassName)
    {
        return aggregateClassName?.ContainingProject?.Name.Replace(".Domain", "");
    }
    /// <summary>
    /// Get Handler Name Space String
    /// </summary>
    /// <param name="projectName"></param>
    /// <param name="ClassFolderName"></param>
    /// <param name="cqrsEnum"></param>
    /// <returns></returns>
    public static string GetHandlerNameSpaceString(string projectName, string ClassFolderName, CqrsEnum cqrsEnum)
    {
        var plurualizeClassFolderName = ClassFolderName.Plurualize();
        var plurualizeCqrsEnum = cqrsEnum.ToString().Plurualize();
        return $"{projectName}.Application.Handlers.{plurualizeClassFolderName}.{plurualizeCqrsEnum}";
    }
    /// <summary>
    /// Get Dto File Path
    /// </summary>
    /// <param name="aggregateClassPath"></param>
    /// <param name="aggregateClassName"></param>
    /// <param name="dtosFolderPath"></param>
    /// <returns></returns>
    public static string GetDtoFilePath(string aggregateClassPath, string aggregateClassName, string dtoName, out string dtosFolderPath)
    {
        var handlerClassPath = PathConst.HandlerPath(aggregateClassPath, aggregateClassName);
        dtosFolderPath = Path.Combine(handlerClassPath, "Queries", @"Dtos\");
        return dtosFolderPath + dtoName + ".cs";
    }
    /// <summary>
    /// Get Queries Folder Path
    /// </summary>
    /// <param name="aggregateClassPath"></param>
    /// <param name="aggregateClassName"></param>
    /// <returns></returns>
    public static string GetQueriesFolderPath(string aggregateClassPath, string aggregateClassName)
    {
        var handlerClassPath = PathConst.HandlerPath(aggregateClassPath, aggregateClassName);
        return Path.Combine(handlerClassPath, "Queries");
    }
}
