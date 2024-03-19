namespace PaymentService.Insfrastructure.Utilities.Grid.Filter
{
    /// <summary>
    /// dynamic grid filter models
    /// </summary>
    public class FilterModel(string propertyName, string filterType, string filter, bool jsonOrXml, string? andOrOperation)
    {
        public string PropertyName { get; set; } = propertyName;
        public string FilterType { get; set; } = filterType;
        public string Filter { get; set; } = filter;
        public bool JsonOrXml { get; set; } = jsonOrXml;
        public string? AndOrOperation { get; set; } = andOrOperation;
    }
}
