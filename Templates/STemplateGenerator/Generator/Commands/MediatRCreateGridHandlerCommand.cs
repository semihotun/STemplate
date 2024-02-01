using EnvDTE;
using Generator.Business.ServiceCollection;
using Generator.Business.VsStore;
using Generator.Consts;
using Generator.Helpers;
namespace Generator.Commands;

/// <summary>
/// Generator menu grid click command
/// </summary>
[Command(PackageIds.MediatRCreateGridHandlerCommand)]
internal class MediatRCreateGridHandlerCommand : BaseCommand<MediatRCreateGridHandlerCommand>
{
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
        if (_vsWritableSettingsStoreManager.GetMySetting(new(SettingsStoreConst.CollectionPath, SettingsStoreConst.DifferentFileSettingName))?.DifferentFile != null)
        {
            difFile = (_vsWritableSettingsStoreManager.GetMySetting(new(SettingsStoreConst.CollectionPath, SettingsStoreConst.DifferentFileSettingName))?.DifferentFile != 0);
        }
        var projectItem = EnvDTEHelper.GetProjectItem(base.Package);
        if (projectItem is not null)
        {
            IncludeToolForm myForm = new(projectItem, difFile);
            myForm.ShowDialog();
        }
    }
}
