using Generator.Business.Dtos;
using Generator.Business.MediatR.Grid.RequestModel;
using Generator.Business.MediatR.Template;
using Generator.Business.MediatR.Template.Models.Base;
using Generator.Business.ServiceCollection;
using Generator.Const;
using Generator.Extensions;
using Generator.Helpers;
using Generator.ManuelMapper;
using Generator.Models;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
namespace Generator.Business.MediatR.Grid;

internal class MediatRCreateGridManager : IMediatRCreateGridManager
{
    private readonly IMediatrTemplate _mediatRTemplate = CustomServiceCollection.MediatrTemplate();
    private readonly IDtoCreatorManager _dtoManager = CustomServiceCollection.DtoManager();
    /// <summary>
    /// Click to GenerateButton
    /// </summary>
    /// <param name="request"></param>
    public async Task<bool> GenerateCodeAsync(GridGenerateVeriables request)
    {
        if (String.IsNullOrEmpty(request.DtoName))
        {
            await VS.MessageBox.ShowWarningAsync(Message.GetWriteDtoName);
            return false;
        }
        //Get selected property type
        var propertyInfoList = request.PropertiesCheckedList.Select(x => x.SplitOnce('=').First())
            .Select(item => new PropertyInfoByClass(request.SemanticAllClassProperties.Values
                .SelectMany(list => list)
                .ToList()
                .Find(x => x.PropertyName.Replace("!.", "").Replace(".", "") == item)?.Type, item))
            .Where(property => property.Type != null).ToList();
        //Get File Path
        var filePath = PathConst.GetDtoFilePath(request.FilePath, request.DbTableName, request.DtoName, out var dtosFolderPath);
        //Create Request
        var fileWriteRequest = request.GetCreateAggregateClassRequest(filePath);
        fileWriteRequest.SetClassProperty(new List<SyntaxPropertyInfo>().AddPagedListFilterModel());
        await WriteFileAllAsync(new(fileWriteRequest, request, filePath, dtosFolderPath, propertyInfoList));
        return true;
    }
    #region Private Class
    /// <summary>
    /// Write File Request and request handler
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    private async Task WriteFileAllAsync(WriteFileAllAsync request)
    {
        //Create Dto
        await _dtoManager.CreateDtoAsync(new CreateDtoRequest(
            request.PropertyInfoList,
            request.GridGenerateVeriables.DtoName,
            request.DtosFolderPath,
            request.GridGenerateVeriables.ProjectName,
            request.DtoFilePath,
            request.GridGenerateVeriables.DbTableName));
        //Create compiledQuery
        var generateCompileQueryDtoAsync = GenerateCompileQueryDtoAsync(request.GridGenerateVeriables,
            request.DtoFilePath);
        //Isnot DifferentFile
        FolderHelper.CreateIfFileNotExsist(request.CreateAggregateClassRequest.CommandOrQueryPath);
        var requestString = GenerateCodeRequestToGetRequestModel(request.CreateAggregateClassRequest,
            request.GridGenerateVeriables);
        //Create Request
        var requestFileTask = GetCreateAddMethodRequestStringAsync(requestString,
            request.CreateAggregateClassRequest);
        //DifferentFile
        if (request.CreateAggregateClassRequest.DifferentFile)
        {
            requestString = requestString with
            {
                RequestHandlerUsingString = _mediatRTemplate.GetDtoRequestHandlerUsing(request.GridGenerateVeriables.GetDtoRequestUsing())
            };
            var requestHandlerFileTask = FileHelper.WriteFileAsync(request.CreateAggregateClassRequest.IRequestHandlerFilePath,
                _mediatRTemplate.GetRequestHandlerString(requestString).FormatCsharpDocumentCode());
            await Task.WhenAll(requestFileTask, requestHandlerFileTask, generateCompileQueryDtoAsync);
            return;
        }
        await Task.WhenAll(requestFileTask, generateCompileQueryDtoAsync);
    }
    /// <summary>
    /// Map model
    /// </summary>
    /// <param name="request"></param>
    /// <param name="gridGenerateVeriables"></param>
    /// <returns></returns>
    private GetRequestModel GenerateCodeRequestToGetRequestModel(CreateAggregateClassRequest request, GridGenerateVeriables gridGenerateVeriables)
    {
        return request.GetRequestModel(
            constructerString: _mediatRTemplate.GetDtoConstructer(),
            requestUsing: _mediatRTemplate.GetDtoRequestUsing(new(gridGenerateVeriables.ProjectName,
            gridGenerateVeriables.DbTableName, request.NameSpaceString, request.DifferentFile)),
            requestHandleMethod: GetRequestHandlerString(gridGenerateVeriables),
            repositoryClassName: gridGenerateVeriables.DbTableName,
            primaryConstructor: _mediatRTemplate.GetDtoPrimaryConstructerParameters());
    }
    /// <summary>
    /// Request file string
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    private async Task GetCreateAddMethodRequestStringAsync(GetRequestModel request, CreateAggregateClassRequest createAggregateClassRequest) =>
         await FileHelper.WriteFileAsync(createAggregateClassRequest.IRequestFilePath, _mediatRTemplate.GetRequestString(request).FormatCsharpDocumentCode());
    /// <summary>
    /// Request handler file string
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    private string GetRequestHandlerString(GridGenerateVeriables request) =>
         $@"return await _cacheService.GetAsync<DataResult<IPagedList<{request.DtoName}>>>(request,async () =>
                {{
                    var query = await Get{request.DtoName}CompiledQueryCompiledQuery.Get(_coreDbContext).ToTableSettings(request.PagedListFilterModel);
                    return new SuccessDataResult<IPagedList<{request.DtoName}>>(query);
                }}, cancellationToken);";
    /// <summary>
    /// Generate Compile Query
    /// </summary>
    /// <param name="request"></param>
    /// <param name="dtoFilePath"></param>
    /// <returns></returns>
    private async Task GenerateCompileQueryDtoAsync(GridGenerateVeriables request, string dtoFilePath)
    {
        var task = Task.Run(() =>
        {
            var compiledQueryPath = dtoFilePath.Replace("Dtos", "CompiledQuery").Replace(request.DtoName, $"{request.DtoName}CompiledQuery");
            FolderHelper.CreateIfFileNotExsist(Path.GetDirectoryName(compiledQueryPath));
            var dbTableNamePlurualize = request.DbTableName.Plurualize();
            var data = @$"using {request.ProjectName}.Application.Handlers.{dbTableNamePlurualize}.Queries.Dtos;
                              using {request.ProjectName}.Domain.AggregateModels;
                              using {request.ProjectName}.Persistence.Context;
                              using Microsoft.EntityFrameworkCore;
                              namespace {request.ProjectName}.Application.Handlers.{dbTableNamePlurualize}.Queries.CompiledQuery;
                              public static class Get{Path.GetFileNameWithoutExtension(compiledQueryPath)}CompiledQuery 
                              {{
                              {"\t"}public static readonly Func<CoreDbContext, IQueryable<{request.DtoName}>> Get = EF.CompileQuery<CoreDbContext, IQueryable<{request.DtoName}>>(Context =>
                                         {"\t\t"}Context.Set<{request.DbTableName}>()
                                         {"\t\t"}{String.Join("\n\t\t\t", request.IncludeCheckBoxList.Select(x => $".Include(x=>x.{x})"))}
                                         {"\t\t\t\t"}.Select(x=> new {request.DtoName}(
                                       {"\t\t\t\t\t"}x.Id,
                                        {"\t\t\t\t\t"}{String.Join(",\n\t\t\t\t\t", request.PropertiesCheckedList.Select(s => s.SplitOnce('=').Last()))}
                                        {"\t\t\t"})));
                              }}";
            File.WriteAllText(compiledQueryPath, data.RemoveFromTap());
        });
        await task;
    }
    #endregion
}