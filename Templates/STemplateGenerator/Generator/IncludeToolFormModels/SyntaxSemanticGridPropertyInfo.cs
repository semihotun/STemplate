namespace Generator.IncludeToolFormModels;
internal class SyntaxSemanticGridPropertyInfo
{
    public SyntaxSemanticGridPropertyInfo(string selectPropertyName,
        string type,
        string @namespace,
        bool ısGeneric,
        string genericNameArgument,
        bool systemProperties,
        string className,
        bool classIsGeneric
        )
    {
        Type = type;
        Namespace = @namespace;
        IsGeneric = ısGeneric;
        GenericNameArgument = genericNameArgument;
        SystemProperties = systemProperties;
        ClassName = className;
        ClassIsGeneric = classIsGeneric;
        PropertyName = className + selectPropertyName;
        if (className is not null)
        {
            if (classIsGeneric)
            {
                SelectPropertyName = className + selectPropertyName;
                PropertyNameInsideClass = $"{className.Replace("!.", "")}{selectPropertyName}=string.Join(',', x.{className}.Select(s => s.{selectPropertyName}))";
            }
            else
            {
                SelectPropertyName = className + "!." + selectPropertyName;
                PropertyNameInsideClass = $"{className.Replace("!.", "")}{selectPropertyName.Replace(".", "")}=x.{className}!.{selectPropertyName}";
            }
        }
        else
        {
            SelectPropertyName = className + selectPropertyName;
            PropertyNameInsideClass = $"{selectPropertyName}=x.{selectPropertyName}";
        }
    }
    public string SelectPropertyName { get; set; }
    public string Type { get; set; }
    public string Namespace { get; set; }
    public bool IsGeneric { get; set; }
    public string GenericNameArgument { get; set; }
    public bool SystemProperties { get; set; }
    public string ClassName { get; set; }
    public string PropertyNameInsideClass { get; set; }
    public bool ClassIsGeneric { get; set; }
    public string PropertyName { get; set; }
}
