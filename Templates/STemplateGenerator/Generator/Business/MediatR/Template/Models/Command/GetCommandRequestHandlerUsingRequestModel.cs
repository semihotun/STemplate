namespace Generator.Business.MediatR.Template.Models.Command;
internal record GetCommandRequestHandlerUsingRequestModel(string ProjectName,
    bool DifferentFile,
    string NameSpaceString,
    bool IsMapper);