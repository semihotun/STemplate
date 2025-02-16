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
using System.Linq;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Text.RegularExpressions;
using System.Text;
using StreamJsonRpc;
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
        //var mapperTask = Task.Run(() => _mapperlyManager.CreateAddMethodRequest(request.CreateMapperlyAddMethodRequest()));
        //Request and handler File Write
        var requestString = await CreateMediatRAddMethodRequestToGetRequestModelAsync(request);
        var getCreateAddMethodRequestString = GetCreateAddMethodRequestString(requestString);
        var requestFileTask = FileHelper.WriteFileAsync(request.IRequestFilePath,
            getCreateAddMethodRequestString);

        var validatorString=GenerateValidator(getCreateAddMethodRequestString, request);
        var validatorTask =FileHelper.WriteFileAsync(request.ValidatorPath,validatorString.FormatCsharpDocumentCode().Replace(", ", ",\r\n\t\t"));
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
            await Task.WhenAll(requestFileTask, requestHandlerFileTask, validatorTask);
            return;
        }
        await Task.WhenAll(requestFileTask, validatorTask);
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
                request.GetClassGenerateMethod([AcceptableMethodEnum.Add, AcceptableMethodEnum.Set]), request),
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
    private async Task<string> GetCreateAddMethodRequestHandlerInnerStringAsync(GetClassGenerateMethod request, CreateAggregateClassRequest createAggregateClass)
    {
        // Mevcut sınıf dosyasını oku
        var classContent = File.ReadAllText(createAggregateClass.ClassPath);
        var syntaxTree = CSharpSyntaxTree.ParseText(classContent);
        var compilation = CSharpCompilation.Create($"{createAggregateClass.ClassName}Compilation")
            .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
            .AddSyntaxTrees(syntaxTree);

        var semanticModel = compilation.GetSemanticModel(syntaxTree);
        var root = syntaxTree.GetCompilationUnitRoot();

        // Sınıfın property'lerini al
        var classDeclaration = root.DescendantNodes().OfType<ClassDeclarationSyntax>()
            .First(c => c.Identifier.Text == createAggregateClass.ClassName);

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

        var createParameters = string.Join(", ", systemProperties
            .Select(p => $"request.{p.Identifier.Text}"));

        return $@"return await _unitOfWork.BeginTransaction(async () =>
                                     {{     
                                            var data = {request.ClassName}.Create({createParameters});
                                            {String.Join("\n", await request.GetClassGenerateMethodStringAsync())}
                                            await _{request.ClassName.MakeFirstLetterLowerCaseWithRegex()}Repository.AddAsync(data); 
                                            await _cacheService.RemovePatternAsync(""{request.ProjectName}:{request.ClassName.Plurualize()}"");
                                            return Result.SuccessResult(Messages.Added);
                                     }});";
    }
    public static string GenerateValidator(string commandCode,CreateAggregateClassRequest request)
    {
        FolderHelper.CreateIfFileNotExsist(request.ValidatorFilePath);
        var syntaxTree = CSharpSyntaxTree.ParseText(commandCode);
        var compilation = CSharpCompilation.Create("CommandCompilation")
            .AddReferences(MetadataReference.CreateFromFile(typeof(object).Assembly.Location))
            .AddSyntaxTrees(syntaxTree);

        var semanticModel = compilation.GetSemanticModel(syntaxTree);

        var recordDeclaration = syntaxTree.GetRoot()
            .DescendantNodes()
            .OfType<RecordDeclarationSyntax>()
            .FirstOrDefault();

        if (recordDeclaration == null)
            throw new ArgumentException("No record declaration found in the provided code");

        var className = recordDeclaration.Identifier.Text;

        var properties = recordDeclaration.ParameterList.Parameters
            .Select(p => p.Identifier.Text)
            .ToList();

        var validationRules = string.Join("\n",
            properties.Select(prop =>
                $@"           RuleFor(x => x.{prop})
               .NotEmpty().WithMessage(""{prop}IsNotEmpty"");"
            ));

        var plurualizeClassFolderName = request.ClassName.Plurualize();
        var namespaceSting= $"{request.ProjectName}.Application.Handlers.{plurualizeClassFolderName}.Validators";
        return $@"
                using {request.NameSpaceString};
                using FluentValidation;
                namespace {namespaceSting};
                public class {className}Validator : AbstractValidator<{className}>
                {{
                    public {className}Validator()
                    {{
                         {validationRules}
                    }}
                }}";
    }
    #endregion
}