using Generator.Business.Mapper;
using Generator.Business.MediatR.Template;
using Generator.Business.MediatR.Template.Models.Base;
using Generator.Business.ServiceCollection;
using Generator.Const;
using Generator.Extensions;
using Generator.Helpers;
using Generator.ManuelMapper;
using Generator.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System.IO;
using System.Linq;
namespace Generator.Business.MediatR.Update;
internal class MediatRCreateUpdateMethodManager : IMediatRCreateUpdateMethodManager
{
    private readonly ICreateAddMapperlyMapperManager _mapperlyManager = CustomServiceCollection.CreateAddMapperlyMapperManager();
    private readonly IMediatrTemplate _mediatRTemplate = CustomServiceCollection.MediatrTemplate();
    /// <summary>
    /// Create Method Request
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task CreateUpdateMethodRequestAsync(CreateAggregateClassRequest request)
    {
        FolderHelper.CreateIfFileNotExsist(request.CommandOrQueryPath);
        if (File.Exists(request.IRequestFilePath) || File.Exists(request.IRequestHandlerFilePath))
        {
            await VS.MessageBox.ShowErrorAsync(Message.MustNotContainFile + $"= {request.IRequestFilePath}");
            return;
        }
        //Mapper Create
        //var mapperTask = Task.Run(() => _mapperlyManager.CreateUpdateMethodRequest(request.CreateMapperlyUpdateMethodRequest()));
        //Request and handler File Write
        var requestString = CreateMediatRUpdateMethodRequestToGetRequestModel(request);
        var requestFileTask = FileHelper.WriteFileAsync(request.IRequestFilePath, GetCreateUpdateMethodRequestString(requestString));
        //Differen File
        if (request.DifferentFile)
        {
            requestString = requestString with { RequestHandlerUsingString = _mediatRTemplate.GetCommandRequestHandlerUsing(request.GetCommandRequestHandlerUsingRequestModel()) };
            var requestHandlerFileTask = FileHelper.WriteFileAsync(request.IRequestHandlerFilePath, GetCreateUpdateMethodRequestHandlerString(requestString));
            await Task.WhenAll(requestFileTask, requestHandlerFileTask);
            return;
        }
        await Task.WhenAll(requestFileTask);
    }
    /// <summary>
    /// Get Create Update Method Request Handler String
    /// </summary>
    /// <param name="requestString"></param>
    /// <returns></returns>
    private string GetCreateUpdateMethodRequestHandlerString(GetRequestModel requestString) =>
        _mediatRTemplate.GetRequestHandlerString(requestString).FormatCsharpDocumentCode().Replace(", ", ",\r\n\t\t");
    /// <summary>
    /// Map CreateMediatRAddMethodRequest to  GetRequestModel
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    private GetRequestModel CreateMediatRUpdateMethodRequestToGetRequestModel(CreateAggregateClassRequest request) =>
        request.GetRequestModel(
            constructerString: _mediatRTemplate.GetCommandConstructorString(request.GetCommandConstructorStringRequestModel()),
            requestUsing: _mediatRTemplate.GetCommandRequestUsing(request.GetCommandRequestUsingRequestModel()),
            requestHandleMethod: GetCreateUpdateMethodRequestHandlerInnerString(request),
            primaryConstructor: _mediatRTemplate.GetCommandHandlerPrimaryConstructorParameters(new(request.ClassName))
            );
    /// <summary>
    /// Request Tempalte
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    private string GetCreateUpdateMethodRequestString(GetRequestModel request) =>
        _mediatRTemplate.GetRequestString(request).FormatCsharpDocumentCode().Replace(", ", ",\r\n\t\t");

    private string GetCreateUpdateMethodRequestHandlerInnerString(CreateAggregateClassRequest request)
    {
        // Mevcut sınıf dosyasını oku
        var classContent = File.ReadAllText(request.ClassPath);
        var syntaxTree = CSharpSyntaxTree.ParseText(classContent);
        var compilation = CSharpCompilation.Create($"{request.ClassName}Compilation")
            .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
            .AddSyntaxTrees(syntaxTree);

        var semanticModel = compilation.GetSemanticModel(syntaxTree);
        var root = syntaxTree.GetCompilationUnitRoot();

        // Sınıfın property'lerini al
        var classDeclaration = root.DescendantNodes().OfType<ClassDeclarationSyntax>()
            .First(c => c.Identifier.Text == request.ClassName);

        var propertyDeclarations = classDeclaration.Members.OfType<PropertyDeclarationSyntax>();

        // System tiplerini filtrele
        var systemProperties = propertyDeclarations.Where(prop =>
        {
            var typeSymbol = semanticModel.GetTypeInfo(prop.Type).Type;
            if (typeSymbol is INamedTypeSymbol namedTypeSymbol)
            {
                var isSystemType = namedTypeSymbol.ContainingNamespace?.ToString().StartsWith("System") ?? false;
                var isNotCollection = !namedTypeSymbol.IsGenericType ||
                                    !namedTypeSymbol.ConstructedFrom.ToString().Contains("System.Collections");
                return isSystemType && isNotCollection;
            }
            return false;
        });

        var updateParameters = string.Join(", ", systemProperties
            .Select(p => $"request.{p.Identifier.Text}"));

        var firstLoverClassName = request.ClassName.MakeFirstLetterLowerCaseWithRegex();
        return $@"return await _unitOfWork.BeginTransaction(async () =>
                                     {{     
                                       var data = await _{firstLoverClassName}Repository.GetAsync(u => u.Id == request.Id);
                                       if(data is not null)
                                       {{
                                           data.Update({updateParameters});                                   
                                           _{firstLoverClassName}Repository.Update(data);
                                           await _cacheService.RemovePatternAsync(""{request.ProjectName}:{request.ClassName.Plurualize()}"");
                                           return Result.SuccessResult(Messages.Updated);
                                       }}
                                       return Result.ErrorResult(Messages.UpdatedError);
                                     }});";
    }
}
