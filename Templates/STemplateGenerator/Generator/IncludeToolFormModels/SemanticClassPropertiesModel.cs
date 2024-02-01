using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
namespace Generator.IncludeToolFormModels;

internal class SemanticClassPropertiesModel(
    IEnumerable<PropertyDeclarationSyntax> classProperties,
    SemanticModel semanticModel,
    string className)
{
    public IEnumerable<PropertyDeclarationSyntax> ClassProperties { get; set; } = classProperties;
    public SemanticModel SemanticModel { get; set; } = semanticModel;
    public string ClassName { get; set; } = className;
}
