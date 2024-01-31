using Generator.Business.MediatR.Template.Models.Base;
using Generator.Business.MediatR.Template.Models.Command;
using Generator.Business.MediatR.Template.Models.Dto;
using Generator.Business.MediatR.Template.Models.Query;
using Generator.Extensions;
using Microsoft.CodeAnalysis;
using System.Linq;
namespace Generator.Business.MediatR.Template;

internal class MediatrTemplate : IMediatrTemplate
{
    /// <summary>
    /// Get Request String
    /// Write Property {String.Join("\n", request.GetClassProperty.Select(x => x.PropertyString)) ?? ""}
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public string GetRequestString(GetRequestModel request)
    {
        return @$"{request.RequestUsingString}
                    namespace  {request.NameSpaceString};
                    public record {request.RequestName} (
                           {String.Join(",\n", request.GetClassProperty.Select(x => x.PrimaryConstructerString))}
                           ): IRequest<{request.MethodReturnTypeName}>;
                           {(!request.DifferentFile ? GetRequestHandlerString(request) : "")}          
                       ";
    }
    /// <summary>
    /// Get RequestHandler String
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public string GetRequestHandlerString(GetRequestModel request)
    {
        return $@"{request.RequestHandlerUsingString}
                    public class {request.RequestName}Handler({request.PrimaryConstructor}) : IRequestHandler<{request.RequestName},{request.MethodReturnTypeName}>
                    {{
                       {request.ConstructorString}
                       public async Task<{request.MethodReturnTypeName}> Handle({request.RequestName} request, CancellationToken cancellationToken)
                       {{
                            {request.RequestHandleMethod}
                       }}
                    }}";
    }
    #region Commands
    public string GetCommandHandlerPrimaryConstructorParameters(GetCommandHandlerPrimaryConstructorParametersRequestModel request)
    {
        return @$"IRepository<{request.RepositoryClassName}>{request.RepositoryClassName.MakeFirstLetterLowerCaseWithRegex()}Repository,
                      ICoreDbContext coreDbContext,
                      ICacheService cacheService";
    }
    /// <summary>
    /// Get Command Constructor
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public string GetCommandConstructorString(GetCommandConstructorStringRequestModel request)
    {
        return $@"{(request.IsMapper ? $"private readonly {request.RepositoryClassName}Mapper _blogMapper = new ();" : "")}
                      private readonly IRepository<{request.RepositoryClassName}> _{request.RepositoryClassName.MakeFirstLetterLowerCaseWithRegex()}Repository = {request.RepositoryClassName.MakeFirstLetterLowerCaseWithRegex()}Repository;
                      private readonly ICoreDbContext _coreDbContext = coreDbContext;
                      private readonly ICacheService _cacheService = cacheService;
                    ";
    }
    /// <summary>
    /// Get Command Request Using
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public string GetCommandRequestUsing(GetCommandRequestUsingRequestModel request)
    {
        return $@"using MediatR;
                  using {request.ProjectName}.Domain.Result;
                  {(!request.IsAggregateUsing ? $"using {request.ProjectName}.Domain.AggregateModels;" : "")}
                  {(request.DifferentFile ? "" : @$"
                  using {request.ProjectName}.Persistence.Context;
                  using {request.ProjectName}.Persistence.GenericRepository;
                  using {request.ProjectName}.Insfrastructure.Utilities.Caching.Redis;
                  using {request.ProjectName}.Application.Constants;
                  {(request.IsMapper ? $"using {request.ProjectName}.Application.Handlers.Mapper;" : "")}
                  ")}";
    }
    /// <summary>
    /// Get Command Request Handler Using
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public string GetCommandRequestHandlerUsing(GetCommandRequestHandlerUsingRequestModel request)
    {
        return $@"using MediatR;
                  using {request.ProjectName}.Domain.Result;
                  using {request.ProjectName}.Persistence.Context;
                  using {request.ProjectName}.Persistence.GenericRepository;
                  using {request.ProjectName}.Domain.AggregateModels;
                  using {request.ProjectName}.Insfrastructure.Utilities.Caching.Redis;                   
                  using {request.ProjectName}.Application.Constants;
                  {(request.IsMapper ? $"using {request.ProjectName}.Application.Handlers.Mapper;" : "")}
                  namespace {request.NameSpaceString};";
    }
    /// <summary>
    /// Properties can be Collection or class
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public bool IsAggregateUsing(IsAggregateUsingRequestModel request)
    {
        var usingList = request.GetClassProperty.First().GetReference().SyntaxTree.GetAllUsing(
            $"{request.ProjectName}.Domain.AggregateModels");
        if (usingList is null)
            return false;
        return usingList.Any(x => x == $"{request.ProjectName}.Domain.AggregateModels");
    }
    #endregion
    #region Dto
    /// <summary>
    /// Get dto request Using
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public string GetDtoRequestHandlerUsing(GetDtoRequestHandlerUsing request)
    {
        var plurualizeFolderName = request.DbTableClassName.Plurualize();
        return $@"using MediatR;
                  using {request.ProjectName}.Domain.Result;
                  using {request.ProjectName}.Insfrastructure.Utilities.Caching.Redis;
                  using {request.ProjectName}.Persistence.Context;
                  using {request.ProjectName}.Insfrastructure.Utilities.Grid.PagedList;
                  using {request.ProjectName}.Application.Handlers.{plurualizeFolderName}.Queries.Dtos;
                  using {request.ProjectName}.Application.Handlers.{plurualizeFolderName}.Queries.CompiledQuery;
                  using {request.ProjectName}.Insfrastructure.Utilities.Grid.Filter;
                  namespace {request.ProjectName}.Application.Handlers.{plurualizeFolderName}.Queries;";
    }
    /// <summary>
    /// Get Dto Request Using
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public string GetDtoRequestUsing(GetDtoRequestUsingRequestModel request)
    {
        var plurualizeFolderName = request.DbTableClassName.Plurualize();
        return $@"using MediatR;
                  using {request.ProjectName}.Insfrastructure.Utilities.Grid.PagedList;
                  using {request.ProjectName}.Domain.Result;
                  using {request.ProjectName}.Application.Handlers.{plurualizeFolderName}.Queries.Dtos;
                  {(request.DifferentFile ? "" : $@"
                  using {request.ProjectName}.Insfrastructure.Utilities.Caching.Redis;
                  using {request.ProjectName}.Persistence.Context;
                  using {request.ProjectName}.Application.Handlers.{plurualizeFolderName}.Queries.CompiledQuery;
                  using {request.ProjectName}.Insfrastructure.Utilities.Grid.Filter;")}
                 ";
    }
    /// <summary>
    /// Get Dto Constructor
    /// </summary>
    /// <returns></returns>
    public string GetDtoConstructer()
    {
        return @"private readonly CoreDbContext _coreDbContext = coreDbContext;
                 private readonly ICacheService _cacheService = cacheService;";
    }
    /// <summary>
    /// Get Dto Primary Constructor Parameters
    /// </summary>
    /// <returns></returns>
    public string GetDtoPrimaryConstructerParameters()
    {
        return "CoreDbContext coreDbContext, ICacheService cacheService";
    }
    #endregion
    #region Queries
    public string GetQueryHandlerPrimaryConstructorParameters(GetQueryHandlerPrimaryConstructorParametersRequestModel request)
    {
        return @$"IRepository<{request.RepositoryClassName}>{request.RepositoryClassName.MakeFirstLetterLowerCaseWithRegex()}Repository,
                      ICacheService cacheService";
    }
    /// <summary>
    /// Get Command Constructor
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public string GetQueryConstructorString(GetQueryConstructorStringRequestModel request)
    {
        return $@"private readonly IRepository<{request.RepositoryClassName}> _{request.RepositoryClassName.MakeFirstLetterLowerCaseWithRegex()}Repository = {request.RepositoryClassName.MakeFirstLetterLowerCaseWithRegex()}Repository;
                      private readonly ICacheService _cacheService = cacheService;
                    ";
    }
    /// <summary>
    /// Get Command Request Using
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public string GetQueryRequestUsing(GetQueryRequestUsingRequestModel request)
    {
        return $@"using MediatR;
                  using {request.ProjectName}.Domain.Result;
                  using {request.ProjectName}.Domain.AggregateModels;
                  {(request.DifferentFile ? "" : @$"
                  using {request.ProjectName}.Persistence.GenericRepository;
                  using {request.ProjectName}.Insfrastructure.Utilities.Caching.Redis;
                  ")}";
    }
    /// <summary>
    /// Get Command Request Handler Using
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public string GetQueryRequestHandlerUsing(GetQueryRequestHandlerUsingRequestModel request)
    {
        return $@"using MediatR;
                  using {request.ProjectName}.Domain.Result;
                  using {request.ProjectName}.Persistence.Context;
                  using {request.ProjectName}.Persistence.GenericRepository;
                  using {request.ProjectName}.Domain.AggregateModels;
                  using {request.ProjectName}.Insfrastructure.Utilities.Caching.Redis;                   
                  namespace {request.NameSpaceString};";
    }
    #endregion
}