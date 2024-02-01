using Generator.Extensions;
using Generator.IncludeToolFormModels;
using Microsoft.CodeAnalysis.CSharp;
using System.IO;
namespace Generator.Helpers;

internal static class SemanticHelper
{
    /// <summary>
    /// Get Semantic Model and classproperties
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="compilationAssemblyName"></param>
    /// <param name="semantiClassPropertiesModels"></param>
    /// <returns></returns>
    internal static bool IsGetSemanticModelWithFilePath(string filePath, string compilationAssemblyName, string className,
     out SemanticClassPropertiesModel semantiClassPropertiesModels)
    {
        if (!File.Exists(filePath))
        {
            semantiClassPropertiesModels = null;
            return false;
        }
        var compilation = CSharpCompilation.Create(compilationAssemblyName);
        var syntaxTree = CSharpSyntaxTree.ParseText(File.ReadAllText(filePath));
        compilation = compilation.AddSyntaxTrees(syntaxTree);
        semantiClassPropertiesModels = new SemanticClassPropertiesModel(
        classProperties: syntaxTree.GetClassProperties(className),
        semanticModel: compilation.GetSemanticModel(syntaxTree),
        className: className);
        return semantiClassPropertiesModels.ClassProperties is not null;
    }
}
