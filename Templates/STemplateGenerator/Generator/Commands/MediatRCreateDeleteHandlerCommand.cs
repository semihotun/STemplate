using EnvDTE;
using Generator.Business.MediatR.Delete;
using Generator.Business.MediatR.Template;
using Generator.Business.ServiceCollection;
using Generator.Business.VsStore;
using Generator.Const;
using Generator.Consts;
using Generator.Extensions;
using Generator.Helpers;
using Generator.Models;
using System.Collections.Generic;
using System.IO;
namespace Generator.Commands;
/// <summary>
/// Generator menü delete button click command
/// </summary>
[Command(PackageIds.MediatRCreateDeleteHandlerCommand)]
internal sealed class MediatRCreateDeleteHandlerCommand : BaseCommand<MediatRCreateDeleteHandlerCommand>
{
    private readonly IMediatRCreateDeleteMethodManager _mediatRCreateDeleteMethodManager = CustomServiceCollection.MediatRCreateDeleteMethodManager();
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
        if (_vsWritableSettingsStoreManager.GetMySetting(new(SettingsStoreConst.CollectionPath, SettingsStoreConst.DifferentFileSettingName))
            ?.DifferentFile != null)
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
                    projectItem.FileNames[0],
                    operation: OperationEnum.Delete,
                    commandOrQuery: CqrsEnum.Command,
                    returnType: "Result",
                    isAggregateUsing: _mediatrTemplate.IsAggregateUsing(new(projectName, classProperties)),
                    isMapper: false,
                    requestName: $"Delete{className}Command"
                    );
                var properties = new List<SyntaxPropertyInfo>();
                properties.AddIdSyntaxPropertyInfo();
                request.SetClassProperty(properties);
                await _mediatRCreateDeleteMethodManager.CreateDeleteMethodRequestAsync(request);
            }
        }
    }
}