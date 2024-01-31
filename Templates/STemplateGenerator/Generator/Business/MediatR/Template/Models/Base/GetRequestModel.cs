using Generator.Models;
using System.Collections.Generic;
namespace Generator.Business.MediatR.Template.Models.Base;
internal record GetRequestModel(
    string ProjectName,
    bool DifferentFile,
    List<SyntaxPropertyInfo> GetClassProperty,
    string NameSpaceString,
    string ClassName,
    string RequestHandleMethod,
    string RequestName,
    string MethodReturnTypeName,
    OperationEnum Operation,
    string RequestUsingString,
    string RequestHandlerUsingString,
    string ConstructorString,
     string PrimaryConstructor,
    string RepositoryClassName
        )
{
    public string RepositoryClassName { get; set; } = RepositoryClassName ?? ClassName;
}