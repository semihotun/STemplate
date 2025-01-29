using EnvDTE;
using Generator.Business.MediatR.Template;
using Generator.Business.MediatR.Update;
using Generator.Business.ServiceCollection;
using Generator.Business.VsStore;
using Generator.Const;
using Generator.Consts;
using Generator.Extensions;
using Generator.Helpers;
using Generator.Models;
using System.IO;
namespace Generator.Commands;

/// <summary>
/// Generator menu Update button click command
/// </summary>
[Command(PackageIds.MediatRCreateUpdateHandlerCommand)]
internal class MediatRCreateUpdateHandlerCommand : BaseCommand<MediatRCreateUpdateHandlerCommand>
{
    private readonly IMediatRCreateUpdateMethodManager _mediatRCreateUpdateMethodManager = CustomServiceCollection.MediatRCreateUpdateMethodManager();
    private readonly IMediatrTemplate _mediatrTemplate = CustomServiceCollection.MediatrTemplate();
    private readonly IVsWritableSettingsStoreManager _vsWritableSettingsStoreManager = CustomServiceCollection.VsWritableSettingsStoreManager();
    /// <summary>
    /// Execute Command
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
    {
        await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
        var difFile = false;
        if (_vsWritableSettingsStoreManager.GetMySetting(new(SettingsStoreConst.CollectionPath,
            SettingsStoreConst.DifferentFileSettingName))?.DifferentFile != null)
        {
            difFile = (_vsWritableSettingsStoreManager.GetMySetting(new(SettingsStoreConst.CollectionPath,
                SettingsStoreConst.DifferentFileSettingName))
                ?.DifferentFile != 0);
        }
        foreach (var projectItem in EnvDTEHelper.GetProjectItems(base.Package))
        {
            if (!projectItem.FileNames[0].Contains(".Domain\\AggregateModels\\"))
            {
                await VS.MessageBox.ShowErrorAsync(Message.PleaseCheckFile);
                continue;
            }
            if (!SyntaxTreeExtension.IsGetClassPropertiesWithFilePath(projectItem?.FileNames[0], out var classProperties))
            {
                await VS.MessageBox.ShowErrorAsync(Message.IsNotProperties + "= " + projectItem?.FileNames[0]);
                return;
            }
            else
            {
                var projectName = PathConst.GetProjectName(projectItem);
                var className = Path.GetFileNameWithoutExtension(projectItem?.FileNames[0]);
                var request = new CreateAggregateClassRequest(
                    className: Path.GetFileNameWithoutExtension(projectItem?.FileNames[0]),
                    projectName: projectName,
                    differentFile: difFile,
                    classPath: projectItem.FileNames[0],
                    operation: OperationEnum.Update,
                    commandOrQuery: CqrsEnum.Command,
                    returnType: "Result",
                    isAggregateUsing: _mediatrTemplate.IsAggregateUsing(new(projectName, classProperties)),
                    isMapper: false,
                    requestName: $"Update{className}Command"
                    );
                request.SetClassProperty(classProperties.CreatePropertiesSourceCode().AddIdSyntaxPropertyInfo());
                await _mediatRCreateUpdateMethodManager.CreateUpdateMethodRequestAsync(request);
            }
        }
    }
}
