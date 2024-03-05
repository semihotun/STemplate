using Generator.Business.MediatR.Template;
using Generator.Business.MediatR.Template.Models.Base;
using Generator.Business.ServiceCollection;
using Generator.Const;
using Generator.Extensions;
using Generator.Helpers;
using Generator.ManuelMapper;
using Generator.Models;
using System.IO;
namespace Generator.Business.MediatR.Delete;
internal class MediatRCreateDeleteMethodManager : IMediatRCreateDeleteMethodManager
{
    private readonly IMediatrTemplate _mediatRTemplate = CustomServiceCollection.MediatrTemplate();
    /// <summary>
    /// Create Delete Method
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task CreateDeleteMethodRequestAsync(CreateAggregateClassRequest request)
    {
        FolderHelper.CreateIfFileNotExsist(request.CommandOrQueryPath);
        if (File.Exists(request.IRequestFilePath) || File.Exists(request.IRequestHandlerFilePath))
        {
            await VS.MessageBox.ShowErrorAsync(Message.MustNotContainFile + $"= {request.IRequestFilePath}");
            return;
        }
        var requestUsing = _mediatRTemplate.GetCommandRequestUsing(request.GetCommandRequestUsingRequestModel());
        //Handler and Request file request model
        var requestString = request.GetRequestModel(
            constructerString: _mediatRTemplate.GetCommandConstructorString(request.GetCommandConstructorStringRequestModel()),
            requestUsing: requestUsing,
            requestHandleMethod: GetCreateDeleteMethodRequestHandlerInnerString(request),
            primaryConstructor: _mediatRTemplate.GetCommandHandlerPrimaryConstructorParameters(new(request.ClassName)));
        //Request File Write
        var requestFileTask = FileHelper.WriteFileAsync(request.IRequestFilePath, GetCreateDeleteMethodRequestString(requestString));
        //if different file write
        if (request.DifferentFile)
        {
            requestString = requestString with
            {
                RequestHandlerUsingString = _mediatRTemplate.GetCommandRequestHandlerUsing(request.GetCommandRequestHandlerUsingRequestModel())
            };
            var requestHandlerFileTask = FileHelper.WriteFileAsync(request.IRequestHandlerFilePath,
                GetCreateDeleteMethodRequestHandlerString(requestString));
            await Task.WhenAll(requestFileTask, requestHandlerFileTask);
            return;
        }
        await Task.WhenAll(requestFileTask);
    }
    #region private
    /// <summary>
    /// Get Create Delete Method Request Handler String
    /// </summary>
    /// <param name="requestString"></param>
    /// <returns></returns>
    private string GetCreateDeleteMethodRequestHandlerString(GetRequestModel requestString) =>
        _mediatRTemplate.GetRequestHandlerString(requestString).FormatCsharpDocumentCode().Replace(", ", ",\r\n\t\t");
    /// <summary>
    /// Get Create Delete Method Request String
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    private string GetCreateDeleteMethodRequestString(GetRequestModel request)
    {
        return _mediatRTemplate.GetRequestString(request).FormatCsharpDocumentCode().Replace(", ", ",\r\n\t\t");
    }
    /// <summary>
    /// Get Create Delete Method Request Handler Inner String
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    private string GetCreateDeleteMethodRequestHandlerInnerString(CreateAggregateClassRequest request)
    {
        var classFirstLetterLoverCase = request.ClassName.MakeFirstLetterLowerCaseWithRegex();
        return $@"return await _unitOfWork.BeginTransaction<Result>(async () =>
                                     {{     
                                        var data = await _{classFirstLetterLoverCase}Repository.GetAsync(p => p.Id == request.Id);
                                        if(data is not null)
                                        {{
                                             data.Deleted = true;
                                            _{classFirstLetterLoverCase}Repository.Update(data);
                                            await _cacheService.RemovePatternAsync(""{request.ProjectName}:{request.ClassName.Plurualize()}"");
                                            return new SuccessResult(Messages.Deleted);
                                        }}
                                        else{{
                                            return new ErrorResult(Messages.DeletedError);
                                        }}                             
                                     }});";
    }
    #endregion
}
