using System.Text.RegularExpressions;

namespace CustomerService.Insfrastructure.Utilities.ServiceBus
{
    public static class FormatterRegex
    {
        /// <summary>
        /// Example input Selectionİtem Underscore=selection-item
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string Underscore(this string input)
        {
            return Regex.Replace(
                Regex.Replace(
                    Regex.Replace(input, @"([\p{Lu}]+)([\p{Lu}][\p{Ll}])", "$1_$2"), @"([\p{Ll}\d])([\p{Lu}])", "$1_$2"), @"[-\s]", "_").ToLower();
        }
    }
}
