using EnvDTE;
using Generator.Business.MediatR.GetById;
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

[Command(PackageIds.MediatRCreateGetByIdHandlerCommand)]
internal class MediatRCreateGetByIdHandlerCommand : BaseCommand<MediatRCreateGetByIdHandlerCommand>
{
    private readonly IMediatRCreateGetByIdMethodManager _mediatRCreateAddMethodManager = CustomServiceCollection.MediatRCreateGetByIdMethodManager();
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
            difFile = (_vsWritableSettingsStoreManager.GetMySetting(new(SettingsStoreConst.CollectionPath, SettingsStoreConst.DifferentFileSettingName))
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
                var className = Path.GetFileNameWithoutExtension(projectItem?.FileNames[0]);
                var projectName = PathConst.GetProjectName(projectItem);
                var request = new CreateAggregateClassRequest(
                    className: className,
                    projectName: projectName,
                    differentFile: difFile,
                    classPath: projectItem.FileNames[0],
                    operation: OperationEnum.Get,
                    commandOrQuery: CqrsEnum.Query,
                    returnType: $"DataResult<{className}>",
                    isAggregateUsing: _mediatrTemplate.IsAggregateUsing(new(projectName, classProperties)),
                    isMapper: false,
                    requestName: $"Get{className}ByIdQuery"
                    );
                request.SetClassProperty(new List<SyntaxPropertyInfo>().AddIdSyntaxPropertyInfo());
                await _mediatRCreateAddMethodManager.CreateGetByIdMethodRequestAsync(request);
            }
        }
    }
}
