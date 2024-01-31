using System.IO;
namespace Generator.Helpers;

internal static class FileHelper
{
    /// <summary>
    /// File Write Async
    /// </summary>
    /// <param name="filePath"></param>
    /// <param name="text"></param>
    /// <returns></returns>
    public static Task WriteFileAsync(string filePath, string text)
    {
        return Task.Run(() => File.WriteAllText(filePath, text));
    }
}