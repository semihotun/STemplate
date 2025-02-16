using Generator.Const;
using Generator.Extensions;
using System.Collections.Generic;
using System.IO;
namespace Generator.Models;

internal class CreateAggregateClassRequest
{
    public CreateAggregateClassRequest(string className,
        string projectName,
        bool differentFile,
        string classPath,
        OperationEnum operation,
        CqrsEnum commandOrQuery,
        string returnType,
        bool isAggregateUsing,
        bool isMapper,
        string requestName,
        string classMainHandlersPath = null,
        string nameSpaceString = null)
    {
        ReturnType = returnType;
        Operation = operation;
        CommandOrQuery = commandOrQuery;
        ClassName = className;
        ProjectName = projectName;
        ClassPath = classPath;
        ClassMainHandlersPath = classMainHandlersPath ?? PathConst.HandlerPath(classPath, className);
        CommandOrQueryPath = Path.Combine(ClassMainHandlersPath, commandOrQuery.ToString().Plurualize());
        RequestName = requestName;
        ValidatorFilePath = Path.Combine(ClassMainHandlersPath, "Validators");
        ValidatorPath = Path.Combine(ValidatorFilePath,$"{RequestName}Validator.cs");
        IRequestFilePath = Path.Combine(CommandOrQueryPath, $"{RequestName}.cs");
        IRequestHandlerFilePath = Path.Combine(CommandOrQueryPath, $"{RequestName}Handler.cs");
        NameSpaceString = nameSpaceString ?? PathConst.GetHandlerNameSpaceString(projectName, className, commandOrQuery);
        DifferentFile = differentFile;
        ClassPath = classPath;
        IsAggregateUsing = isAggregateUsing;
        IsMapper = isMapper;
    }
    public void SetClassProperty(List<SyntaxPropertyInfo> property)
    {
        GetClassProperty = property;
    }
    public string ValidatorFilePath { get;  }
    public string ValidatorPath { get;  }
    public string ClassName { get; }
    public string ProjectName { get; }
    public bool DifferentFile { get; } = false;
    public string ClassPath { get; }
    public bool IsAggregateUsing { get; }
    public bool IsMapper { get; }
    public string ClassMainHandlersPath { get; }
    public string CommandOrQueryPath { get; }
    public string NameSpaceString { get; }
    public string IRequestHandlerFilePath { get; }
    public string IRequestFilePath { get; }
    public string RequestName { get; }
    public string ReturnType { get; }
    public OperationEnum Operation { get; }
    public CqrsEnum CommandOrQuery { get; }
    public List<SyntaxPropertyInfo> GetClassProperty { get; private set; }
}
