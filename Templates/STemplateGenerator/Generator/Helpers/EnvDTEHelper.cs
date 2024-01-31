using EnvDTE;
using System.Collections.Generic;

namespace Generator.Helpers;

internal static class EnvDTEHelper
{
    /// <summary>
    /// Get Project Item
    /// </summary>
    /// <param name="Package"></param>
    /// <returns></returns>
    public static ProjectItem GetProjectItem(AsyncPackage Package)
    {
        var dte = Package.GetService<EnvDTE.DTE, EnvDTE80.DTE2>();
        if (dte == null) return null;
        var selectedItem = (Array)dte?.ToolWindows.SolutionExplorer?.SelectedItems;
        if (selectedItem?.Length != 1) return null;
        if (selectedItem.GetValue(0) is not UIHierarchyItem selectedHierarchyItem) return null;
        if (selectedHierarchyItem?.Object is not EnvDTE.ProjectItem projectItem) return null;
        if (projectItem?.FileCount == 0) return null;
        return projectItem;
    }
    /// <summary>
    /// Get Selected all PROJECT Item
    /// </summary>
    /// <param name="Package"></param>
    /// <returns></returns>
    public static List<ProjectItem> GetProjectItems(AsyncPackage Package)
    {
        ThreadHelper.ThrowIfNotOnUIThread();
        var dte = Package.GetService<EnvDTE.DTE, EnvDTE80.DTE2>();
        if (dte == null) return null;
        var selectedItems = (Array)dte?.ToolWindows.SolutionExplorer?.SelectedItems;
        if (selectedItems?.Length == 0) return null;
        var projectItems = new List<EnvDTE.ProjectItem>();
        foreach (var selectedItem in selectedItems)
        {
            if (selectedItem is UIHierarchyItem selectedHierarchyItem && selectedHierarchyItem?.Object is ProjectItem projectItem)
            {
                projectItems.Add(projectItem);
            }
        }
        return projectItems;
    }
}
