using Generator.IncludeToolFormModels;
using System.Collections.Generic;
namespace Generator.Business.MediatR.Grid.RequestModel;

internal record GridGenerateVeriables(
    IEnumerable<string> IncludeCheckBoxList,
    List<string> PropertiesCheckedList,
    /// <summary>
    /// TextBox text NAME
    /// </summary>
    string DtoName,
    Dictionary<string, IEnumerable<SyntaxSemanticGridPropertyInfo>> SemanticAllClassProperties,
    string FilePath,
    /// <summary>
    /// Selected Class Name
    /// </summary>
    string DbTableName,
    string ProjectName,
    bool DifferentFile);
