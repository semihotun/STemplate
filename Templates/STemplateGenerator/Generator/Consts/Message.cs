using System.Globalization;
namespace Generator.Const;
/// <summary>
/// Consts Message
/// </summary>
internal static class Message
{
    internal static CultureInfo CultureInfo => new("en-US");
    internal static string IsNotDescribeClass => "Please define the class file name and class must match";
    internal static string IsNotProperties => "No Properties";
    internal static string PleaseCheckFile => "Only classes in the AggregateModel file can create";
    internal static string MustNotContainFile => "We cannot create this request for security reasons because the file has already created this request, if you still want to create it please delete the request and handler files";
    internal static string GetWriteDtoName => "Write your dto name";
    internal static string DtoIsExsist => "Dto already exists";
}
