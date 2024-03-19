namespace CatalogService.Insfrastructure.Utilities.MediatorBehaviour.Validation.Exceptions
{
    /// <summary>
    /// validation eror model
    /// </summary>
    /// <param name="PropertyName"></param>
    /// <param name="ErrorMessage"></param>
    public record ValidationData(string PropertyName, string ErrorMessage);
}
