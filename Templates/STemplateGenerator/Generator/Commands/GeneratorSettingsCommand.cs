using Generator.Business.ServiceCollection;
using Generator.Business.VsStore;
using Generator.Consts;

namespace Generator.Commands;

/// <summary>
/// Generator Setting Command
/// </summary>
[Command(PackageIds.GeneratorSettingsCommand)]
internal class GeneratorSettingsCommand : BaseCommand<GeneratorSettingsCommand>
{
    private readonly IVsWritableSettingsStoreManager settingsManager = CustomServiceCollection.VsWritableSettingsStoreManager();
    /// <summary>
    /// initial Setting get collection
    /// </summary>
    /// <returns></returns>
    protected override Task InitializeCompletedAsync()
    {
        this.Command.Visible = false;
        var data = settingsManager.GetMySetting(new(SettingsStoreConst.CollectionPath, SettingsStoreConst.DifferentFileSettingName));
        if (data != null)
        {
            this.Command.Checked = data.DifferentFile != 0;
        }
        else
        {
            this.Command.Checked = false;
        }

        return Task.CompletedTask;
    }
    /// <summary>
    /// Execute Command
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected override void Execute(object sender, EventArgs e)
    {
        this.Command.Checked = !this.Command.Checked;
        int value = this.Command.Checked ? 1 : 0;
        settingsManager.SetMySetting(new(SettingsStoreConst.CollectionPath, SettingsStoreConst.DifferentFileSettingName, value));
    }
}