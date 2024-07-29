using Generator.Business.Mapper.Models;
using Generator.Extensions;
using Generator.Helpers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.IO;
namespace Generator.Business.Mapper;
/// <summary>
/// Create Add Mapperly Mapper Manager
/// </summary>
internal class CreateAddMapperlyMapperManager : ICreateAddMapperlyMapperManager
{
    /// <summary>
    /// Create MapperlyMethod and using
    /// </summary>
    /// <param name="request"></param>
    public void CreateAddMethodRequest(CreateMapperlyAddMethodRequest request)
    {
        if (!File.Exists(request.FilePath))
        {
            FolderHelper.CreateIfFileNotExsist(request.FolderPath);
            File.WriteAllText(request.FilePath, request.MapperText.FormatCsharpDocumentCode());
        }
        else
        {
            var classContent = File.ReadAllText(request.FilePath);
            if (!classContent.Contains($"{request.ClassName}Mapper"))
            {
                File.WriteAllText(request.FilePath, request.MapperText.FormatCsharpDocumentCode());
            }
            else
            {
                AddSyntaxTreeMethod(request, classContent);
            }
        }
    }
    /// <summary>
    /// Create MapperlyMethod and using
    /// </summary>
    /// <param name="request"></param>
    public void CreateUpdateMethodRequest(CreateMapperlyUpdateMethodRequest request)
    {
        if (!File.Exists(request.FilePath))
        {
            FolderHelper.CreateIfFileNotExsist(request.FolderPath);
            File.WriteAllText(request.FilePath, request.MapperText.FormatCsharpDocumentCode());
        }
        else
        {
            var classContent = File.ReadAllText(request.FilePath);
            if (!classContent.Contains($"{request.ClassName}Mapper"))
            {
                File.WriteAllText(request.FilePath, request.MapperText.FormatCsharpDocumentCode());
            }
            else
            {
                UpdateSyntaxTreeMethod(request, classContent);
            }
        }
    }
    /// <summary>
    /// Add Syntax Tree for Mapperly
    /// </summary>
    /// <param name="request"></param>
    /// <param name="classContent"></param>
    private void AddSyntaxTreeMethod(CreateMapperlyAddMethodRequest request, string classContent)
    {
        if (classContent.Contains($"{request.RequestName}To{request.ClassName}"))
        {
            return;
        }
        var syntaxTree = CSharpSyntaxTree.ParseText(classContent);
        var root = syntaxTree.GetCompilationUnitRoot();
        if (root.IsGetTargetNamespace(request.NamespaceString, out var targetNamespace) &&
            targetNamespace.IsGetTargetClass($"{request.ClassName}Mapper", out var targetClass) &&
            !targetClass.IsGetTargetMethods($"{request.RequestName}To{request.ClassName}", out _))
        {
            var newMethodDeclaration = (SyntaxFactory.MethodDeclaration(
                returnType: SyntaxFactory.ParseTypeName($"{request.ClassName}"),
                identifier: SyntaxFactory.Identifier($"{request.RequestName}To{request.ClassName}"))
                .WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword), SyntaxFactory.Token(SyntaxKind.StaticKeyword), SyntaxFactory.Token(SyntaxKind.PartialKeyword)))
                .WithParameterList(SyntaxFactory.ParameterList(
                    SyntaxFactory.SingletonSeparatedList(
                        SyntaxFactory.Parameter(
                            SyntaxFactory.Identifier(request.FirstLoverClassName))
                        .WithType(SyntaxFactory.ParseTypeName(request.RequestName)))))
                 .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken))// ; için  Bracket için  .WithBody(SyntaxFactory.Block())
                 ).NormalizeWhitespace();
            //Sırayı bozma text replace atarak çalışıyor
            var newRoot = root.ReplaceNode(targetClass, targetClass.AddMembers(newMethodDeclaration));
            var newUsingRoot = (targetNamespace.AddRootUsing(newRoot,
                    $"{request.NamespaceString}",
                    $"{request.ProjectName}.Domain.AggregateModels",
                    "Riok.Mapperly.Abstractions")).NormalizeWhitespace();
            newRoot = newRoot.ReplaceNode(newRoot, newUsingRoot);
            var newSyntaxTree = syntaxTree.WithRootAndOptions(newRoot, syntaxTree.Options);
            File.WriteAllText(request.FilePath, newSyntaxTree.ToString().FormatCsharpDocumentCode());
        }
    }
    /// <summary>
    /// Add Update Syntax Tree for Mapperly
    /// </summary>
    /// <param name="request"></param>
    /// <param name="classContent"></param>
    private void UpdateSyntaxTreeMethod(CreateMapperlyUpdateMethodRequest request, string classContent)
    {
        //if (classContent.Contains($"{request.RequestName}To{request.ClassName}"))
        //{
        //    return;
        //}
        //var syntaxTree = CSharpSyntaxTree.ParseText(classContent);
        //var root = syntaxTree.GetCompilationUnitRoot();
        //if (root.IsGetTargetNamespace(request.NamespaceString, out var targetNamespace) &&
        //    targetNamespace.IsGetTargetClass($"{request.ClassName}Mapper", out var targetClass) &&
        //    !targetClass.IsGetTargetMethods($"{request.RequestName}To{request.ClassName}", out _))
        //{
        //    var newMethodDeclaration = SyntaxFactory.MethodDeclaration(
        //       returnType: SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.VoidKeyword)),
        //       identifier: SyntaxFactory.Identifier($"{request.RequestName}To{request.ClassName}"))
        //       .WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword), SyntaxFactory.Token(SyntaxKind.StaticKeyword), SyntaxFactory.Token(SyntaxKind.PartialKeyword)))
        //       .WithParameterList(SyntaxFactory.ParameterList(
        //           SyntaxFactory.SeparatedList<ParameterSyntax>(new SyntaxNodeOrToken[]{
        //    SyntaxFactory.Parameter(SyntaxFactory.Identifier(request.RequestName.MakeFirstLetterLowerCaseWithRegex()))
        //        .WithType(SyntaxFactory.ParseTypeName(request.RequestName)),
        //    SyntaxFactory.Token(SyntaxKind.CommaToken),
        //    SyntaxFactory.Parameter(SyntaxFactory.Identifier(request.FirstLoverClassName))
        //        .WithType(SyntaxFactory.ParseTypeName(request.ClassName))
        //           })))
        //       .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken))
        //       .NormalizeWhitespace();

        //    var newRoot = root.ReplaceNode(targetClass, targetClass.AddMembers(newMethodDeclaration));
        //    var newUsingRoot = (targetNamespace.AddRootUsing(newRoot,
        //            $"{request.NamespaceString}",
        //            $"{request.ProjectName}.Domain.AggregateModels",
        //            "Riok.Mapperly.Abstractions")).NormalizeWhitespace();
        //    newRoot = newRoot.ReplaceNode(newRoot, newUsingRoot);
        //    var newSyntaxTree = syntaxTree.WithRootAndOptions(newRoot, syntaxTree.Options);
        //    File.WriteAllText(request.FilePath, newSyntaxTree.ToString().FormatCsharpDocumentCode());
        //}
        if (classContent.Contains($"{request.RequestName}To{request.ClassName}"))
        {
            return;
        }
        var syntaxTree = CSharpSyntaxTree.ParseText(classContent);
        var root = syntaxTree.GetCompilationUnitRoot();
        if (root.IsGetTargetNamespace(request.NamespaceString, out var targetNamespace) &&
            targetNamespace.IsGetTargetClass($"{request.ClassName}Mapper", out var targetClass) &&
            !targetClass.IsGetTargetMethods($"{request.RequestName}To{request.ClassName}", out _))
        {
            var newMethodDeclaration = (SyntaxFactory.MethodDeclaration(
                returnType: SyntaxFactory.ParseTypeName($"{request.ClassName}"),
                identifier: SyntaxFactory.Identifier($"{request.RequestName}To{request.ClassName}"))
                .WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PublicKeyword), SyntaxFactory.Token(SyntaxKind.StaticKeyword), SyntaxFactory.Token(SyntaxKind.PartialKeyword)))
                .WithParameterList(SyntaxFactory.ParameterList(
                    SyntaxFactory.SingletonSeparatedList(
                        SyntaxFactory.Parameter(
                            SyntaxFactory.Identifier(request.FirstLoverClassName))
                        .WithType(SyntaxFactory.ParseTypeName(request.RequestName)))))
                 .WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken))// ; için  Bracket için  .WithBody(SyntaxFactory.Block())
                 ).NormalizeWhitespace();
            //Sırayı bozma text replace atarak çalışıyor
            var newRoot = root.ReplaceNode(targetClass, targetClass.AddMembers(newMethodDeclaration));
            var newUsingRoot = (targetNamespace.AddRootUsing(newRoot,
                    $"{request.NamespaceString}",
                    $"{request.ProjectName}.Domain.AggregateModels",
                    "Riok.Mapperly.Abstractions")).NormalizeWhitespace();
            newRoot = newRoot.ReplaceNode(newRoot, newUsingRoot);
            var newSyntaxTree = syntaxTree.WithRootAndOptions(newRoot, syntaxTree.Options);
            File.WriteAllText(request.FilePath, newSyntaxTree.ToString().FormatCsharpDocumentCode());
        }
    }
}
