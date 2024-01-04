namespace AdminIdentityService.Insfrastructure.Utilities.Grid.Filter
{
    /// <summary>
    /// dynamic grid filter models
    /// </summary>
    public class FilterModel
    {
        public string PropertyName { get; set; }
        public string FilterType { get; set; }
        public string Filter { get; set; }
        public bool JsonOrXml { get; set; }
        public string? AndOrOperation { get; set; }
        public FilterModel(string propertyName, string filterType, string filter, bool jsonOrXml, string? andOrOperation)
        {
            PropertyName = propertyName;
            FilterType = filterType;
            Filter = filter;
            JsonOrXml = jsonOrXml;
            AndOrOperation = andOrOperation;
        }
    }
}
