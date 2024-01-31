using Generator.Models;
using System.Collections.Generic;

namespace Generator.Business.MediatR.Template.Models.Command;
internal record GetCommandRequestUsingRequestModel(string ProjectName,
    bool DifferentFile,
    List<SyntaxPropertyInfo> GetClassProperty,
    bool IsAggregateUsing,
    bool IsMapper);