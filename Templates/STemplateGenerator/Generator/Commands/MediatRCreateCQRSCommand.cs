namespace Generator.Commands;
/// <summary>
/// Generator menü create command and query
/// </summary>
[Command(PackageIds.MediatRCreateCQRSCommand)]
internal sealed class MediatRCreateCQRSCommand : BaseCommand<MediatRCreateCQRSCommand>
{
    /// <summary>
    /// Execute Command
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
    {
        await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
        await VS.Commands.ExecuteAsync(PackageGuids.Generator, PackageIds.MediatRCreateAddHandlerCommand, null);
        await VS.Commands.ExecuteAsync(PackageGuids.Generator, PackageIds.MediatRCreateDeleteHandlerCommand, null);
        await VS.Commands.ExecuteAsync(PackageGuids.Generator, PackageIds.MediatRCreateUpdateHandlerCommand, null);
        await VS.Commands.ExecuteAsync(PackageGuids.Generator, PackageIds.MediatRCreateGetByIdHandlerCommand, null);
    }
}