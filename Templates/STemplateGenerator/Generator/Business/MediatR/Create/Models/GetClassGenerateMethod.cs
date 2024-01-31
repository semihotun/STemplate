using System.Collections.Generic;
namespace Generator.Business.MediatR.Create.Models;

internal record GetClassGenerateMethod(string ProjectName,
    string ClassName,
    string ClassPath,
    List<AcceptableMethodEnum> AcceptableMethodNamePrefix,
    string RequestName);