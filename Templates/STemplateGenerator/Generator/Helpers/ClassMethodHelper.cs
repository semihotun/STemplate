using Generator.Business.MediatR.Create.Models;
using Generator.Extensions;
using Generator.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Generator.Helpers;
internal static class ClassMethodHelper
{
    /// <summary>
    /// To use the methods of the classes inside the mediatr handle method
    /// The reason it is so long is to check if the method properties have the same name so that personalized voids are not created.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public static async Task<List<string>> GetClassGenerateMethodStringAsync(this GetClassGenerateMethod request)
    {
        var result = new List<string>();
        if (CSharpSyntaxTree.ParseText(File.ReadAllText(request.ClassPath)).GetCompilationUnitRoot()
            .IsGetTargetNamespace($"{request.ProjectName}.Domain.AggregateModels", out var targetNamespace) &&
            targetNamespace.IsGetTargetClass($"{request.ClassName}", out var targetClass))
        {
            var propertyList = targetClass.Members.OfType<PropertyDeclarationSyntax>().CreatePropertiesSourceCode();
            var taskList = new List<Task>();
            foreach (var method in targetClass.Members.OfType<MethodDeclarationSyntax>()
                .Where(method => method.Modifiers.Any(modifier => modifier.IsKind(SyntaxKind.PublicKeyword)))
                .ToList())
            {
                taskList.Add(Task.Run(() =>
                {
                    var setVoidParameters = method.ParameterList.Parameters
                    .Select(parameter => new MethodParametersObjectForMediatrAddHandler(
                         "request." + parameter.Identifier.ValueText.MakeFirstLetterUpperCase(),
                         parameter.Identifier.ValueText.MakeFirstLetterUpperCase(),
                         propertyList.Any(property =>
                            property.Type.ToString()
                            .Replace("?", "") == parameter.Type.ToString().Replace("?", "") &&
                            property.Text == parameter.Identifier.ValueText.MakeFirstLetterUpperCase())
                    )).ToList();
                    //Accaptable method can be changed
                    if (request.AcceptableMethodNamePrefix.Any(x => x == AcceptableMethodEnum.Add) &&
                        method.Identifier.ToString().StartsWith(nameof(AcceptableMethodEnum.Add), StringComparison.OrdinalIgnoreCase))
                    {
                        GetClassGenerateMethodAddMethodString(setVoidParameters, method, propertyList, result);
                    }
                    else if (request.AcceptableMethodNamePrefix.Any(x => x == AcceptableMethodEnum.Set) &&
                        method.Identifier.ToString().StartsWith(nameof(AcceptableMethodEnum.Set), StringComparison.OrdinalIgnoreCase))
                    {
                        GetClassGenerateMethodSetMethodString(setVoidParameters, result, method);
                    }
                }));
            }
            await Task.WhenAll(taskList);
        }
        return [.. result.OrderBy(x => x)];
    }
    /// <summary>
    /// check if all the parameters of the method in the class are in the properties and return a result accordingly.
    /// </summary>
    /// <param name="setVoidParameters"></param>
    /// <param name="result"></param>
    /// <param name="method"></param>
    private static void GetClassGenerateMethodSetMethodString(
        List<MethodParametersObjectForMediatrAddHandler> setVoidParameters,
        List<string> result,
        MethodDeclarationSyntax method
        )
    {
        var isMatching = setVoidParameters.Where(x => x.IsMatching).Select(y => y.Text);
        if (method.ParameterList.Parameters.Count == isMatching.Count())
        {
            var parametersString = string.Join(", ", isMatching);
            result.Add($"data.{method.Identifier}({parametersString});");
            return;
        }
    }
    /// <summary>
    /// check if all the parameters of the method in the class are in the properties and return a result accordingly.
    /// </summary>
    /// <param name="setVoidParameters"></param>
    /// <param name="method"></param>
    /// <param name="propertyList"></param>
    /// <param name="result"></param>
    private static void GetClassGenerateMethodAddMethodString(
        List<MethodParametersObjectForMediatrAddHandler> setVoidParameters,
        MethodDeclarationSyntax method,
        List<SyntaxPropertyInfo> propertyList,
        List<string> result)
    {
        var collectionInfo = propertyList
            .Find(info => info.Text == $"{method.Identifier.Value.ToString().Replace("Add", "")}");
        //İs not Generic Type
        if (collectionInfo.Type is not GenericNameSyntax genericNameSyntax) return;
        var index = setVoidParameters
            .FindIndex(x => x.ValueText == genericNameSyntax.TypeArgumentList.Arguments.FirstOrDefault()?.ToString());
        //Not found parameters
        if (index == -1) return;
        //Because generator is x lambda expression
        setVoidParameters[index] = new MethodParametersObjectForMediatrAddHandler(
            Text: "x",
            ValueText: setVoidParameters[index].ValueText,
            IsMatching: true);
        var isMatching = setVoidParameters.Where(x => x.IsMatching).Select(y => y.Text);
        if (method.ParameterList.Parameters.Count == isMatching.Count())
        {
            result.Add($"request.{collectionInfo.Text}.ToList().ForEach(x => data.{method.Identifier}({string.Join(", ", isMatching)}));");
        }
    }
}
