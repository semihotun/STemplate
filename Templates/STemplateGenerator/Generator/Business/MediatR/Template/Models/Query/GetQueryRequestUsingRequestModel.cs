using Generator.Models;
using System.Collections.Generic;

namespace Generator.Business.MediatR.Template.Models.Query;
internal record GetQueryRequestUsingRequestModel(string ProjectName,
    bool DifferentFile,
    List<SyntaxPropertyInfo> GetClassProperty);