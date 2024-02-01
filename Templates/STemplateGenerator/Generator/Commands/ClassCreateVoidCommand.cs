using EnvDTE;
using Generator.Business.Classes;
using Generator.Business.ServiceCollection;
using Generator.Const;
using Generator.Extensions;
using Generator.Helpers;
using System.IO;

namespace Generator.Commands
{
    [Command(PackageIds.ClassCreateVoidCommand)]
    internal class ClassCreateVoidCommand : BaseCommand<ClassCreateVoidCommand>
    {
        private readonly IClassesMethodManager _classesMethodManager = CustomServiceCollection.ClassesMethodManager();
        protected override async Task ExecuteAsync(OleMenuCmdEventArgs e)
        {
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
            foreach (var projectItem in EnvDTEHelper.GetProjectItems(base.Package))
            {
                if (projectItem.FileCount <= 0) { continue; }
                if (!projectItem.FileNames[0].Contains(".Domain\\AggregateModels\\"))
                {
                    await VS.MessageBox.ShowErrorAsync(Message.PleaseCheckFile);
                    continue;
                }
                if (!SyntaxTreeExtension.IsGetClassPropertiesWithFilePath(projectItem?.FileNames[0], out _))
                {
                    await VS.MessageBox.ShowErrorAsync(Message.IsNotProperties + "= " + projectItem?.FileNames[0]);
                    return;
                }
                else
                {
                    _classesMethodManager.GenerateVoid(new(
                        ClassName: Path.GetFileNameWithoutExtension(projectItem?.FileNames[0]),
                        ClassPath: projectItem?.FileNames[0],
                        ProjectName: PathConst.GetProjectName(projectItem)
                        ));
                }
            }
        }
    }
}