using Generator.Business.MediatR.Template;
using Generator.Business.MediatR.Template.Models.Base;
using Generator.Business.ServiceCollection;
using Generator.Const;
using Generator.Extensions;
using Generator.Helpers;
using Generator.ManuelMapper;
using Generator.Models;
using System.IO;

namespace Generator.Business.MediatR.GetById;

internal class MediatRCreateGetAllListMethodManager : IMediatRCreateGetAllListMethodManager
{
    private readonly IMediatrTemplate _mediatRTemplate = CustomServiceCollection.MediatrTemplate();
    /// <summary>
    /// Create Get By Id Method Request Async
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task CreateGetAllListMethodRequestAsync(CreateAggregateClassRequest request)
    {
        FolderHelper.CreateIfFileNotExsist(request.CommandOrQueryPath);
        if (File.Exists(request.IRequestFilePath) || File.Exists(request.IRequestHandlerFilePath))
        {
            await VS.MessageBox.ShowErrorAsync(Message.MustNotContainFile + $"= {request.IRequestFilePath}");
            return;
        }
        var requestUsing = _mediatRTemplate.GetQueryRequestUsing(request.GetQueryRequestUsingRequestModel());
        //Handler and Request Create String
        var constructorString = _mediatRTemplate.GetQueryConstructorString(request.GetQueryConstructorStringRequestModel());
        var requestString = request.GetRequestModel(
            constructerString: constructorString,
            requestUsing: requestUsing,
            requestHandleMethod: GetCreateGetByIdRequestHandlerInnerString(request),
            primaryConstructor: _mediatRTemplate.GetQueryHandlerPrimaryConstructorParameters(new(request.ClassName)));
        //Request File Write
        var requestFileTask = FileHelper.WriteFileAsync(request.IRequestFilePath, GetCreateGetByIdMethodRequestString(requestString));
        //if different file write
        if (request.DifferentFile)
        {
            requestString = requestString with
            {
                RequestHandlerUsingString = _mediatRTemplate.GetQueryRequestHandlerUsing(request.GetQueryRequestHandlerUsingRequestModel())
            };
            var requestHandlerFileTask = FileHelper.WriteFileAsync(request.IRequestHandlerFilePath, GetCreateGetByIdMethodRequestHandlerString(requestString));
            await Task.WhenAll(requestFileTask, requestHandlerFileTask);
            return;
        }
        await Task.WhenAll(requestFileTask);
    }
    /// <summary>
    /// Get Create Delete Method Request Handler String
    /// </summary>
    /// <param name="requestString"></param>
    /// <returns></returns>
    private string GetCreateGetByIdMethodRequestHandlerString(GetRequestModel requestString) =>
        _mediatRTemplate.GetRequestHandlerString(requestString).FormatCsharpDocumentCode().Replace(", ", ",\r\n\t\t");
    /// <summary>
    /// Get Create Delete Method Request String
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    private string GetCreateGetByIdMethodRequestString(GetRequestModel request)
    {
        return _mediatRTemplate.GetRequestString(request).FormatCsharpDocumentCode().Replace(", ", ",\r\n\t\t");
    }
    /// <summary>
    /// Get Create Delete Method Request Handler Inner String
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    private string GetCreateGetByIdRequestHandlerInnerString(CreateAggregateClassRequest request)
    {
        var firstLoverClassName = request.ClassName.MakeFirstLetterLowerCaseWithRegex();
        return $@"return await _cacheService.GetAsync(request,async () =>
                {{
                    var data = await _{firstLoverClassName}Repository.ToListAsync();
                    return Result.SuccessDataResult(data!);
                }}, cancellationToken);";
    }
}
