namespace Generator.Business.Classes.Models;

internal record SyntaxSemanticPropertyInfo(
    string SelectPropertyName,
    string Type,
    string @Namespace,
    bool IsGeneric,
    string GenericNameArgument
    );
