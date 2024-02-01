using Generator.Const;
using Generator.Models;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Generator.Extensions;

internal static class SyntaxTreeExtension
{
    /// <summary>
    /// Format Document c# code with Microsoft.CodeAnalysis.CSharp
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public static string FormatCsharpDocumentCode(this string text)
    {
        return CSharpSyntaxTree.ParseText(text)
            .GetCompilationUnitRoot()
            .NormalizeWhitespace()
            .ToFullString();
    }
    /// <summary>
    /// İs Contain Namespace
    /// </summary>
    /// <param name="root"></param>
    /// <param name="namespaceString"></param>
    /// <param name="targetNamespace"></param>
    /// <returns></returns>
    internal static bool IsGetTargetNamespace(this CompilationUnitSyntax root, string namespaceString, out NamespaceDeclarationSyntax targetNamespace)
    {
        targetNamespace = root.DescendantNodes().OfType<NamespaceDeclarationSyntax>()
            .FirstOrDefault(ns => ns.Name.ToString() == namespaceString);
        return targetNamespace is not null;
    }
    /// <summary>
    /// is contain class
    /// </summary>
    /// <param name="targetNamespace"></param>
    /// <param name="className"></param>
    /// <param name="targetClass"></param>
    /// <returns></returns>
    internal static bool IsGetTargetClass(this NamespaceDeclarationSyntax targetNamespace, string className, out ClassDeclarationSyntax targetClass)
    {
        targetClass = targetNamespace.DescendantNodes().OfType<ClassDeclarationSyntax>()
            .FirstOrDefault(c => c.Identifier.ValueText == className);
        return targetClass is not null;
    }
    /// <summary>
    /// is contain method
    /// </summary>
    /// <param name="targetClass"></param>
    /// <param name="className"></param>
    /// <param name="targetMethod"></param>
    /// <returns></returns>
    internal static bool IsGetTargetMethods(this ClassDeclarationSyntax targetClass, string className, out MethodDeclarationSyntax targetMethod)
    {
        targetMethod = targetClass.Members.OfType<MethodDeclarationSyntax>()
            .FirstOrDefault(m => m.Identifier.ValueText == className);
        return targetMethod is not null;
    }
    /// <summary>
    /// Add File using
    /// </summary>
    /// <param name="namespaceRoot"></param>
    /// <param name="namespaceToAddArray"></param>
    /// <returns></returns>
    internal static CompilationUnitSyntax AddRootUsing(this NamespaceDeclarationSyntax namespaceRoot, CompilationUnitSyntax root, params string[] namespaceToAddArray)
    {
        foreach (var namespaceToAdd in namespaceToAddArray)
        {
            var newUsingDirective = SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(namespaceToAdd));
            if (!root.Usings.AddRange(namespaceRoot.Usings)
                .Any(u => u.Name.ToString() == newUsingDirective.Name.ToString()))
            {
                root = root.AddUsings(newUsingDirective);
            }
        }
        return root;
    }
    /// <summary>
    /// Get all using
    /// </summary>
    /// <param name="tree"></param>
    /// <param name="namespaceString"></param>
    /// <returns></returns>
    internal static List<string> GetAllUsing(this SyntaxTree tree, string namespaceString)
    {
        var root = tree.GetCompilationUnitRoot();
        if (IsGetTargetNamespace(root, namespaceString, out var namespaceSyntax))
        {
            return (root.Usings.AddRange(namespaceSyntax.Usings)).Select(x => x.UsingKeyword.Text).ToList();
        }
        return null;
    }
    /// <summary>
    /// Get Class Properties With File Path
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    internal static bool IsGetClassPropertiesWithFilePath(string filePath, out IEnumerable<PropertyDeclarationSyntax> classProperties)
    {
        classProperties = filePath.GetSyntaxTree().GetClassProperties(Path.GetFileNameWithoutExtension(filePath));
        return classProperties is not null;
    }
    /// <summary>
    /// Get SyntaxTree
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    internal static SyntaxTree GetSyntaxTree(this string filePath)
    {
        return CSharpSyntaxTree.ParseText(File.ReadAllText(filePath));
    }
    /// <summary>
    /// Do you want to Record add code //|| m is RecordDeclarationSyntax recordDeclaration && recordDeclaration.Identifier.ValueText == className
    /// </summary>
    /// <param name="syntaxTree"></param>
    /// <param name="className"></param>
    /// <returns></returns>
    internal static IEnumerable<PropertyDeclarationSyntax> GetClassProperties(this SyntaxTree syntaxTree, string className)
    {
        var classOrRecordDeclaration = syntaxTree.GetRoot().DescendantNodes().OfType<MemberDeclarationSyntax>()
            .FirstOrDefault(m =>
                m is ClassDeclarationSyntax classDeclaration && classDeclaration.Identifier.ValueText == className
            );
        if (classOrRecordDeclaration is not ClassDeclarationSyntax classDeclaration)
        {
            VS.MessageBox.ShowWarning(Message.IsNotDescribeClass);
            return null;
        }
        return classDeclaration.Members.OfType<PropertyDeclarationSyntax>();
    }
    /// <summary>
    ///  Generate Property Source Code
    /// </summary>
    /// <param name="properties"></param>
    /// <returns></returns>
    internal static List<SyntaxPropertyInfo> CreatePropertiesSourceCode(this IEnumerable<PropertyDeclarationSyntax> properties)
    {
        var propertiesString = new List<SyntaxPropertyInfo>();
        foreach (var property in properties)
        {
            propertiesString.Add(new SyntaxPropertyInfo(property.Type, property.Identifier.ValueText));
        }
        return propertiesString;
    }
    /// <summary>
    /// Add Guid Id and propertiesSourceCode to propertystring
    /// </summary>
    /// <param name="propertiesSourceCode"></param>
    /// <returns></returns>
    public static List<SyntaxPropertyInfo> AddIdSyntaxPropertyInfo(this List<SyntaxPropertyInfo> propertiesSourceCode)
    {
        propertiesSourceCode.Add(new SyntaxPropertyInfo(SyntaxFactory.ParseTypeName("System.Guid"), "Id"));
        return propertiesSourceCode;
    }
    /// <summary>
    /// Add PagedList Filter Model
    /// </summary>
    /// <param name="propertiesSourceCode"></param>
    /// <returns></returns>
    public static List<SyntaxPropertyInfo> AddPagedListFilterModel(this List<SyntaxPropertyInfo> propertiesSourceCode)
    {
        propertiesSourceCode.Add(new SyntaxPropertyInfo(
            SyntaxFactory.ParseTypeName("PagedListFilterModel"),
            "PagedListFilterModel"));
        return propertiesSourceCode;
    }
    /// <summary>
    /// Primary Constructer with Generate Property Source Code
    /// </summary>
    /// <param name="properties"></param>
    /// <returns></returns>
    internal static string CreatePrimaryConstructerPropertiesSourceCode(this IEnumerable<PropertyDeclarationSyntax> properties)
    {
        var propertiesString = new List<string>();
        foreach (var property in properties)
        {
            propertiesString.Add($"{property.Type} {property.Identifier.ValueText}, \n");
        }
        return String.Join(",\n", propertiesString);
    }
}