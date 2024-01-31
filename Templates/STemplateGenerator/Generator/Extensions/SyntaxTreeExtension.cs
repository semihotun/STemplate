﻿using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
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
}