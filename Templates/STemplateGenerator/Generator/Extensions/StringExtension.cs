using Generator.Const;
using Microsoft.CodeAnalysis;
using System.Collections.Generic;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using System.Text.RegularExpressions;
namespace Generator.Extensions;

public static class StringExtension
{
    /// <summary>
    /// First Letter Lover case
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string MakeFirstLetterLowerCaseWithRegex(this string input)
    {
        return Regex.Replace(input, "^[A-Z]", match => match.Value == "I" ? "i" : match.Value.ToLower());
    }
    /// <summary>
    /// All trap cleaner
    /// </summary>
    /// <param name="classText"></param>
    /// <returns></returns>
    public static string RemoveFromTap(this string classText)
    {
        const string reduceMultiSpace = "[ ]{2,}";
        var line = Regex.Replace(classText.Replace("\r", " "), reduceMultiSpace, " ");
        return " " + line.TrimStart();
    }
    /// <summary>
    /// Frit letter upper case
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    public static string MakeFirstLetterUpperCase(this string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;
        return Regex.Replace(input, @"^\w", match => match.Value == "ı" ? "I" : match.Value.ToUpper());
    }
    /// <summary>
    /// plural suffix
    /// </summary>
    /// <param name="text"></param>
    /// <param name="cultureInfo"></param>
    /// <returns></returns>
    public static string Plurualize(this string text, CultureInfo cultureInfo = null)
    {
        if (cultureInfo is not null)
        {
            return PluralizationService.CreateService(cultureInfo).Pluralize(text);
        }
        else
        {
            return PluralizationService.CreateService(Message.CultureInfo).Pluralize(text);
        }
    }
}
