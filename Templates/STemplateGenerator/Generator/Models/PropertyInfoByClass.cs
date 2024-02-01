namespace Generator.Models;
internal class PropertyInfoByClass(string type, string text)
{
    public string Type { get; } = type;
    public string Text { get; } = text;
    public string PropertyString { get; } = $"public {type} {text} {{ get; }} = {text};";
    public string PrimaryConstructerString { get; } = $"{type} {text}";
}