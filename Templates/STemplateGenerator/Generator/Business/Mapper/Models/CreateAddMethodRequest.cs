using Generator.Extensions;
using System.IO;
namespace Generator.Business.Mapper.Models;

internal record CreateMapperlyAddMethodRequest
{
    public CreateMapperlyAddMethodRequest(string className, string folderPath, string projectName, string requestName)
    {
        RequestName = requestName;
        ClassName = className;
        FolderPath = folderPath;
        ProjectName = projectName;
        NamespaceString = $"{projectName}.Application.Handlers.Mapper";
        FirstLoverClassName = className.MakeFirstLetterLowerCaseWithRegex();
        PlurualizeClassName = className.Plurualize();
        MapperText = $@"using Riok.Mapperly.Abstractions;
                            using {projectName}.Application.Handlers.{PlurualizeClassName}.Commands;
                            using {projectName}.Domain.AggregateModels;
                            namespace {NamespaceString}
                            {{
                                [Mapper]
                                public static partial class {className}Mapper
                                {{
                                   public static partial {className} {requestName}To{className}({requestName} {FirstLoverClassName});
                                }}
                            }}";
        FilePath = Path.Combine(folderPath, $"{className}Mapper.cs");
    }
    public string ClassName { get; set; }
    public string FolderPath { get; set; }
    public string ProjectName { get; set; }
    public string NamespaceString { get; set; }
    public string FirstLoverClassName { get; set; }
    public string MapperText { get; set; }
    public string FilePath { get; set; }
    public string PlurualizeClassName { get; set; }
    public string RequestName { get; set; }
}
internal record CreateMapperlyUpdateMethodRequest
{
    public CreateMapperlyUpdateMethodRequest(string className, string folderPath, string projectName, string requestName)
    {
        RequestName = requestName;
        ClassName = className;
        FolderPath = folderPath;
        ProjectName = projectName;
        NamespaceString = $"{projectName}.Application.Handlers.Mapper";
        FirstLoverClassName = className.MakeFirstLetterLowerCaseWithRegex();
        PlurualizeClassName = className.Plurualize();
        MapperText = $@"using Riok.Mapperly.Abstractions;
                            using {projectName}.Application.Handlers.{PlurualizeClassName}.Commands;
                            using {projectName}.Domain.AggregateModels;
                            namespace {NamespaceString}
                            {{
                                [Mapper]
                                public static partial class {className}Mapper
                                {{
                                   public static partial void {requestName}To{className}({requestName} {FirstLoverClassName},{className} {FirstLoverClassName} );
                                }}
                            }}";
        FilePath = Path.Combine(folderPath, $"{className}Mapper.cs");
    }
    public string ClassName { get; set; }
    public string FolderPath { get; set; }
    public string ProjectName { get; set; }
    public string NamespaceString { get; set; }
    public string FirstLoverClassName { get; set; }
    public string MapperText { get; set; }
    public string FilePath { get; set; }
    public string PlurualizeClassName { get; set; }
    public string RequestName { get; set; }
}