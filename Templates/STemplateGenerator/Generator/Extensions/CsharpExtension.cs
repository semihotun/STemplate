using System.Collections;
using System.Collections.Generic;
using System.Linq;
namespace Generator.Extensions;

internal static class CsharpExtension
{
    /// <summary>
    /// Foreach extension
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <param name="act"></param>
    /// <returns></returns>
    public static IEnumerable<T> ForEach<T>(this IEnumerable<T> array, Action<T> act)
    {
        foreach (var i in array)
            act(i);
        return array;
    }
    /// <summary>
    /// Foreach extension
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="arr"></param>
    /// <param name="act"></param>
    /// <returns></returns>
    public static IEnumerable<T> ForEach<T>(this IEnumerable arr, Action<T> act)
    {
        return arr.Cast<T>().ForEach<T>(act);
    }
    /// <summary>
    /// Foreach extension
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="RT"></typeparam>
    /// <param name="array"></param>
    /// <param name="func"></param>
    /// <returns></returns>
    public static IEnumerable<RT> ForEach<T, RT>(this IEnumerable<T> array, Func<T, RT> func)
    {
        var list = new List<RT>();
        foreach (var i in array)
        {
            var obj = func(i);
            if (obj != null)
                list.Add(obj);
        }
        return list;
    }
    /// <summary>
    /// Split once
    /// </summary>
    /// <param name="source"></param>
    /// <param name="separator"></param>
    /// <returns></returns>
    public static string[] SplitOnce(this string source, char separator)
    {
        int index = source.IndexOf(separator);
        if (index == -1)
        {
            return [source];
        }
        return [source.Substring(0, index), source.Substring(index + 1)];
    }
}
