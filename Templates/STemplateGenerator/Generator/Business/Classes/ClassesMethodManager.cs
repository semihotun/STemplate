using Generator.Business.Classes.Models;
using Generator.Extensions;
using Generator.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace Generator.Business.Classes;

internal class ClassesMethodManager : IClassesMethodManager
{
    public void GenerateVoid(GenerateVoidRequestModel request)
    {
        if (!File.Exists(request.ClassPath))
            return;

        var compilation = CSharpCompilation.Create($"{request.ClassName}Compilation");
        var syntaxTree = CSharpSyntaxTree.ParseText(File.ReadAllText(request.ClassPath));
        compilation = compilation.AddSyntaxTrees(syntaxTree);

        var root = syntaxTree.GetCompilationUnitRoot();
        var namespaceString = $"{request.ProjectName}.Domain.AggregateModels";

        if (root.IsGetTargetNamespace(namespaceString, out var targetNamespace) &&
            targetNamespace.IsGetTargetClass($"{request.ClassName}", out var targetClass))
        {
            var methodDeclarationSyntax = targetClass.Members.OfType<MethodDeclarationSyntax>();
            var aggregateAllClasses = FolderHelper.GetFolderClassesName(Path.GetDirectoryName(request.ClassPath));
            var properties = GetMethodDeclarationSyntax(
                 syntaxTree.GetClassProperties(request.ClassName),
                 compilation.GetSemanticModel(syntaxTree),
                 aggregateAllClasses,
                 methodDeclarationSyntax);

            var newMethodDeclaration = targetClass.AddMembers(properties.ToArray());
            var newRoot = root.ReplaceNode(targetClass, newMethodDeclaration);
            var newSyntaxTree = syntaxTree.WithRootAndOptions(newRoot, syntaxTree.Options);
            File.WriteAllText(request.ClassPath, newSyntaxTree.ToString().FormatCsharpDocumentCode());
        }
    }
    private IEnumerable<MethodDeclarationSyntax> GetMethodDeclarationSyntax(
    IEnumerable<PropertyDeclarationSyntax> classProperties,
    SemanticModel semanticModel,
    string[] aggregateAllClasses,
    IEnumerable<MethodDeclarationSyntax> methodDeclarationSyntax
    )
    {
        var result = new List<MethodDeclarationSyntax>();
        foreach (var property in classProperties)
        {
            var typeSymbol = semanticModel.GetTypeInfo(property.Type).Type;
            if (typeSymbol is INamedTypeSymbol namedTypeSymbol)
            {
                var namespaceString = typeSymbol.ContainingNamespace?.ToString() ?? typeSymbol.BaseType.Name.GetType().Namespace ?? "";
                if (namedTypeSymbol.OriginalDefinition.SpecialType == SpecialType.System_Nullable_T)
                {
                    namedTypeSymbol = namedTypeSymbol.TypeArguments[0] as INamedTypeSymbol;
                }
                if (namedTypeSymbol.IsGenericType)
                {
                    if (methodDeclarationSyntax.Any(x => x.Identifier.ValueText == $"Add{property.Identifier.Text}"))
                    {
                        continue;
                    }
                    result.Add(GetCreateAddMethodCollectionString(property.Identifier.Text, namedTypeSymbol.TypeArguments[0].Name));
                    continue;
                }
                if (aggregateAllClasses.Any(x => x == namedTypeSymbol.Name))
                {
                    if (methodDeclarationSyntax.Any(x => x.Identifier.ValueText == $"Set{property.Identifier.Text}"))
                    {
                        continue;
                    }
                    result.Add(GetCreateSetModelVoidString(property.Identifier.Text, namedTypeSymbol.Name));
                }
            }
        }
        return result;
    }
    private MethodDeclarationSyntax GetCreateSetModelVoidString(string propertyName, string type)
    {
        var firstLetterLowerCase = type.MakeFirstLetterLowerCaseWithRegex();
        return SyntaxFactory.MethodDeclaration(SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.VoidKeyword)), $"Set{propertyName}")
            .WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword)))
            .WithParameterList(
                SyntaxFactory.ParameterList(
                    SyntaxFactory.SingletonSeparatedList(
                        SyntaxFactory.Parameter(SyntaxFactory.Identifier(firstLetterLowerCase))
                        .WithType(SyntaxFactory.ParseTypeName(type + "?"))
                    )
                )
            )
            .WithBody(SyntaxFactory.Block(
                SyntaxFactory.ExpressionStatement(
                    SyntaxFactory.AssignmentExpression(
                        SyntaxKind.SimpleAssignmentExpression,
                        SyntaxFactory.IdentifierName(propertyName),
                        SyntaxFactory.IdentifierName(firstLetterLowerCase))
                )
            )).NormalizeWhitespace();
    }
    private MethodDeclarationSyntax GetCreateAddMethodCollectionString(string collectionName, string genericParameter)
    {
        var genericParameterFirstLower = genericParameter.MakeFirstLetterLowerCaseWithRegex();
        return SyntaxFactory.MethodDeclaration(SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.VoidKeyword)), $"Add{collectionName}")
         .WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword)))
         .WithParameterList(
            SyntaxFactory.ParameterList(
                SyntaxFactory.SingletonSeparatedList(
                    SyntaxFactory.Parameter(SyntaxFactory.Identifier(genericParameterFirstLower))
                     .WithType(SyntaxFactory.ParseTypeName(genericParameter + "?"))
                )
            )
         )
         .WithBody(SyntaxFactory.Block(
             SyntaxFactory.IfStatement(
                 SyntaxFactory.BinaryExpression(
                     SyntaxKind.NotEqualsExpression,
                     SyntaxFactory.IdentifierName(genericParameterFirstLower),
                     SyntaxFactory.LiteralExpression(SyntaxKind.NullLiteralExpression)
                 ),
                 SyntaxFactory.Block(
                     SyntaxFactory.ExpressionStatement(
                         SyntaxFactory.InvocationExpression(
                             SyntaxFactory.MemberAccessExpression(SyntaxKind.SimpleMemberAccessExpression,
                             SyntaxFactory.IdentifierName(collectionName),
                             SyntaxFactory.IdentifierName("Add")))
                         .WithArgumentList(SyntaxFactory.ArgumentList(
                             SyntaxFactory.SingletonSeparatedList(SyntaxFactory.Argument(SyntaxFactory.IdentifierName(genericParameterFirstLower)))))
                     )
                 )
             )
         )).NormalizeWhitespace();
    }
}