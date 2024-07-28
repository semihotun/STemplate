namespace Generator.Models;
internal class PropertyInfoByClass(string type, string text)
{
    public string Type { get; } = type;
    public string Text { get; } = text;
    public string PropertyString { get; } = $"public {(type.EndsWith("?") == true ? type: type + "?")} {text} {{ get; set; }}";
    public string PrimaryConstructerString { get; } = $"{type} {text}";
}