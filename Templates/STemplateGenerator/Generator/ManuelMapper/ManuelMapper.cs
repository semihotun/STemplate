using Generator.Business.Mapper.Models;
using Generator.Business.MediatR.Create.Models;
using Generator.Business.MediatR.Grid.RequestModel;
using Generator.Business.MediatR.Template.Models.Base;
using Generator.Business.MediatR.Template.Models.Command;
using Generator.Business.MediatR.Template.Models.Dto;
using Generator.Business.MediatR.Template.Models.Query;
using Generator.Extensions;
using Generator.Models;
using System.Collections.Generic;
using System.IO;

namespace Generator.ManuelMapper;

internal static class ManuelMapper
{
    public static GetCommandRequestHandlerUsingRequestModel GetCommandRequestHandlerUsingRequestModel(this CreateAggregateClassRequest request)
    {
        return new GetCommandRequestHandlerUsingRequestModel(
            ProjectName: request.ProjectName,
            DifferentFile: request.DifferentFile,
            NameSpaceString: request.NameSpaceString,
            IsMapper: request.IsMapper);
    }
    public static CreateMapperlyAddMethodRequest CreateMapperlyAddMethodRequest(this CreateAggregateClassRequest request)
    {
        return new CreateMapperlyAddMethodRequest(request.ClassName,
        Path.Combine(request.ClassMainHandlersPath, "Mappers"), request.ProjectName, request.RequestName);
    }
    public static CreateMapperlyUpdateMethodRequest CreateMapperlyUpdateMethodRequest(this CreateAggregateClassRequest request)
    {
        return new CreateMapperlyUpdateMethodRequest(request.ClassName,
        Path.Combine(request.ClassMainHandlersPath, "Mappers"), request.ProjectName, request.RequestName);
    }
    public static GetCommandConstructorStringRequestModel GetCommandConstructorStringRequestModel(this CreateAggregateClassRequest request)
    {
        return new GetCommandConstructorStringRequestModel(RepositoryClassName: request.ClassName, IsMapper: request.IsMapper);
    }
    public static GetRequestModel GetRequestModel(this CreateAggregateClassRequest request,
       string constructerString = null,
       string requestUsing = null,
       string requestHandleMethod = null,
       string requestHandlerUsingString = null,
       string repositoryClassName = null,
       string primaryConstructor = null)
    {
        return new GetRequestModel(
            ProjectName: request.ProjectName,
            DifferentFile: request.DifferentFile,
            GetClassProperty: request.GetClassProperty,
            NameSpaceString: request.NameSpaceString,
            ClassName: request.ClassName,
            RequestHandleMethod: requestHandleMethod,
            RequestName: request.RequestName,
            MethodReturnTypeName: request.ReturnType,
            Operation: request.Operation,
            RequestUsingString: requestUsing,
            RequestHandlerUsingString: requestHandlerUsingString,
            ConstructorString: constructerString,
            PrimaryConstructor: primaryConstructor,
            RepositoryClassName: repositoryClassName);
    }
    public static GetCommandRequestUsingRequestModel GetCommandRequestUsingRequestModel(this CreateAggregateClassRequest request)
    {
        return new GetCommandRequestUsingRequestModel(ProjectName: request.ProjectName,
            DifferentFile: request.DifferentFile,
            GetClassProperty: request.GetClassProperty, request.IsAggregateUsing,
            IsMapper: request.IsMapper);
    }
    public static GetClassGenerateMethod GetClassGenerateMethod(this CreateAggregateClassRequest request, List<AcceptableMethodEnum> acceptableMethodNamePrefix)
    {
        return new GetClassGenerateMethod(ProjectName: request.ProjectName,
            ClassName: request.ClassName,
            ClassPath: request.ClassPath,
            AcceptableMethodNamePrefix: acceptableMethodNamePrefix,
            RequestName: request.RequestName);
    }
    public static GetQueryRequestUsingRequestModel GetQueryRequestUsingRequestModel(this CreateAggregateClassRequest request)
    {
        return new GetQueryRequestUsingRequestModel(ProjectName: request.ProjectName,
            DifferentFile: request.DifferentFile,
            GetClassProperty: request.GetClassProperty);
    }
    public static GetQueryConstructorStringRequestModel GetQueryConstructorStringRequestModel(this CreateAggregateClassRequest request)
    {
        return new GetQueryConstructorStringRequestModel(RepositoryClassName: request.ClassName);
    }
    public static GetQueryRequestHandlerUsingRequestModel GetQueryRequestHandlerUsingRequestModel(this CreateAggregateClassRequest request)
    {
        return new GetQueryRequestHandlerUsingRequestModel(
            ProjectName: request.ProjectName,
            DifferentFile: request.DifferentFile,
            NameSpaceString: request.NameSpaceString);
    }
    public static GetDtoRequestHandlerUsing GetDtoRequestUsing(this GridGenerateVeriables request)
    {
        return new GetDtoRequestHandlerUsing(ProjectName: request.ProjectName, DbTableClassName: request.DbTableName);
    }
    public static CreateAggregateClassRequest GetCreateAggregateClassRequest(this GridGenerateVeriables gridGenerateVeriables, string dtoFilePath)
    {
        return new CreateAggregateClassRequest(
                   className: gridGenerateVeriables.DtoName,
                   projectName: gridGenerateVeriables.ProjectName,
                   differentFile: gridGenerateVeriables.DifferentFile,
                   classPath: dtoFilePath,
                   operation: OperationEnum.Get,
                   commandOrQuery: CqrsEnum.Query,
                   returnType: $"Result<PagedList<{gridGenerateVeriables.DtoName}>>",
                   isAggregateUsing: false,
                   isMapper: false,
                   requestName: $"Get{gridGenerateVeriables.DtoName}Query",
                   classMainHandlersPath: dtoFilePath.Replace($"Queries\\Dtos\\{gridGenerateVeriables.DtoName}.cs", ""),
                   nameSpaceString: $"{gridGenerateVeriables.ProjectName}.Application.Handlers.{gridGenerateVeriables.DbTableName.Plurualize()}.Queries");
    }
}
