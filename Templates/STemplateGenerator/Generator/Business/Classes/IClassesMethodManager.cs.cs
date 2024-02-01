using Generator.Business.Classes.Models;

namespace Generator.Business.Classes;

internal interface IClassesMethodManager
{
    void GenerateVoid(GenerateVoidRequestModel request);
}
