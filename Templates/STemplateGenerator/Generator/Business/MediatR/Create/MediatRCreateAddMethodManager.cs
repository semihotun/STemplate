using Generator.Business.Mapper;
using Generator.Business.MediatR.Create.Models;
using Generator.Business.MediatR.Template;
using Generator.Business.MediatR.Template.Models.Base;
using Generator.Business.ServiceCollection;
using Generator.Const;
using Generator.Extensions;
using Generator.Helpers;
using Generator.ManuelMapper;
using Generator.Models;
using System.IO;
using System.Threading.Tasks;
namespace Generator.Business.MediatR.Create;

/// <summary>
/// Create Source code Add handler
/// </summary>
internal class MediatRCreateAddMethodManager : IMediatRCreateAddMethodManager
{
    private readonly ICreateAddMapperlyMapperManager _mapperlyManager = CustomServiceCollection.CreateAddMapperlyMapperManager();
    private readonly IMediatrTemplate _mediatRTemplate = CustomServiceCollection.MediatrTemplate();
    /// <summary>
    /// Save File and create mapper
    /// </summary>
    /// <param name="request"></param>
    public async Task CreateAddMethodRequestAsync(CreateAggregateClassRequest request)
    {
        FolderHelper.CreateIfFileNotExsist(request.CommandOrQueryPath);
        if (File.Exists(request.IRequestFilePath) || File.Exists(request.IRequestHandlerFilePath))
        {
            await VS.MessageBox.ShowErrorAsync(Message.MustNotContainFile + $"= {request.IRequestFilePath}");
            return;
        }
        //Mapper Create
        var mapperTask = Task.Run(() => _mapperlyManager.CreateAddMethodRequest(request.CreateMapperlyAddMethodRequest()));
        //Request and handler File Write
        var requestString = await CreateMediatRAddMethodRequestToGetRequestModelAsync(request);
        var requestFileTask = FileHelper.WriteFileAsync(request.IRequestFilePath,
            GetCreateAddMethodRequestString(requestString));
        if (request.DifferentFile)
        {
            //requestHandlerUsing
            requestString = requestString with
            {
                RequestHandlerUsingString = _mediatRTemplate.GetCommandRequestHandlerUsing(
                    request.GetCommandRequestHandlerUsingRequestModel())
            };
            var requestHandlerFileTask = FileHelper.WriteFileAsync(request.IRequestHandlerFilePath,
                GetCreateAddMethodRequestHandler(requestString));
            await Task.WhenAll(mapperTask, requestFileTask, requestHandlerFileTask);
            return;
        }
        await Task.WhenAll(mapperTask, requestFileTask);
    }
    #region Private
    /// <summary>
    /// Map CreateMediatRAddMethodRequest to  GetRequestModel
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    private async Task<GetRequestModel> CreateMediatRAddMethodRequestToGetRequestModelAsync(CreateAggregateClassRequest request)
    {
        return request.GetRequestModel(
            constructerString: _mediatRTemplate.GetCommandConstructorString(request.GetCommandConstructorStringRequestModel()),
            requestUsing: _mediatRTemplate.GetCommandRequestUsing(request.GetCommandRequestUsingRequestModel()),
            requestHandleMethod: await GetCreateAddMethodRequestHandlerInnerStringAsync(
                request.GetClassGenerateMethod([AcceptableMethodEnum.Add, AcceptableMethodEnum.Set])),
            null, null,
            primaryConstructor: _mediatRTemplate.GetCommandHandlerPrimaryConstructorParameters(new(request.ClassName))
            );
    }
    /// <summary>
    /// All Request Handler Code string
    /// </summary>
    /// <param name="requestString"></param>
    /// <returns></returns>
    private string GetCreateAddMethodRequestHandler(GetRequestModel requestString) =>
        _mediatRTemplate.GetRequestHandlerString(requestString).FormatCsharpDocumentCode().Replace(", ", ",\r\n\t\t");
    /// <summary>
    /// Request Tempalte
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    private string GetCreateAddMethodRequestString(GetRequestModel request) =>
        _mediatRTemplate.GetRequestString(request).FormatCsharpDocumentCode().Replace(", ", ",\r\n\t\t");
    /// <summary>
    /// Handler Template
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    private async Task<string> GetCreateAddMethodRequestHandlerInnerStringAsync(GetClassGenerateMethod request)
    {
        return $@"return await _unitOfWork.BeginTransaction<Result>(async () =>
                                     {{     
                                            var data = _{request.ClassName.MakeFirstLetterLowerCaseWithRegex()}Mapper.{request.RequestName}To{request.ClassName}(request);
                                            {String.Join("\n", await request.GetClassGenerateMethodStringAsync())}
                                            await _{request.ClassName.MakeFirstLetterLowerCaseWithRegex()}Repository.AddAsync(data); 
                                            await _cacheService.RemovePatternAsync(""{request.ProjectName}:{request.ClassName.Plurualize()}"");
                                            return new SuccessResult(Messages.Added);
                                     }});";
    }
    #endregion
}