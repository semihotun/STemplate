namespace Generator.Business.MediatR.Template.Models.Dto;
internal record GetDtoRequestUsingRequestModel(string ProjectName,
    string DbTableClassName,
    string NameSpaceString,
    bool DifferentFile);