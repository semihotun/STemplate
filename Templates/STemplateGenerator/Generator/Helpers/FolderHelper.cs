using System.IO;
using System.Linq;

namespace Generator.Helpers;

internal static class FolderHelper
{
    /// <summary>
    /// Check Folder
    /// </summary>
    /// <param name="path"></param>
    public static void CreateIfFileNotExsist(string path)
    {
        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }
    /// <summary>
    /// Get FOLDER Classes name
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string[] GetFolderClassesName(string path)
    {
        if (Directory.Exists(path))
        {
            return Directory.GetFiles(path)
                .Select(x => Path.GetFileNameWithoutExtension(x))
                .ToArray();
        }
        return [];
    }
}
