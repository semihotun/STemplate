using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Generic;

namespace Generator.Business.MediatR.Template.Models.Base;
internal record IsAggregateUsingRequestModel(string ProjectName, IEnumerable<PropertyDeclarationSyntax> GetClassProperty);