using Generator.Models;
using System.Collections.Generic;
namespace Generator.Business.MediatR.Grid.RequestModel;

internal record CreateDtoRequest(List<PropertyInfoByClass> Properties,
    string FileName,
    string FolderPath,
    string ProjectName,
    string FilePath,
    string DbTableName);