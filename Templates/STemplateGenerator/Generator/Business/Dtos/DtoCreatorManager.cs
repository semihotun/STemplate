using Generator.Business.MediatR.Grid.RequestModel;
using Generator.Const;
using Generator.Extensions;
using Generator.Helpers;
using Generator.Models;
using System.IO;
using System.Linq;

namespace Generator.Business.Dtos;
internal class DtoCreatorManager : IDtoCreatorManager
{
    /// <summary>
    /// Create Dto
    /// </summary>
    /// <param name="request"></param>
    public async Task CreateDtoAsync(CreateDtoRequest request)
    {
        FolderHelper.CreateIfFileNotExsist(Path.GetDirectoryName(request.FolderPath));
        if (File.Exists(request.FolderPath))
        {
            await VS.MessageBox.ShowWarningAsync(Message.DtoIsExsist);
            return;
        }
        File.WriteAllText(request.FilePath,
         $@"namespace {PathConst.GetHandlerNameSpaceString(request.ProjectName, request.DbTableName.Plurualize(), CqrsEnum.Query)}.Dtos;
                            public record {request.FileName}(Guid Id ,{String.Join(",", request.Properties.Select(x => x.PrimaryConstructerString))}){{
                                   public Guid Id {{get;}} = Id;
                                   {String.Join("\n", request.Properties.Select(x => x.PropertyString))}
                                }}
                            ".FormatCsharpDocumentCode());
    }
}