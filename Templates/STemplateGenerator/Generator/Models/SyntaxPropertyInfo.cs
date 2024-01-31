using Microsoft.CodeAnalysis.CSharp.Syntax;
namespace Generator.Models;
internal class SyntaxPropertyInfo(TypeSyntax type, string text)
{
    public TypeSyntax Type { get; } = type;
    public string Text { get; } = text;
    public string PropertyString { get; } = $"public {type} {text} {{ get; }} = {text};";
    public string PrimaryConstructerString { get; } = $"{type} {text}";
}