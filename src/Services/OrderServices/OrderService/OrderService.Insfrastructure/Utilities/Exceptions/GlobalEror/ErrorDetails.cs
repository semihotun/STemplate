using Newtonsoft.Json;

namespace OrderService.Insfrastructure.Utilities.Exceptions.GlobalEror
{
    /// <summary>
    /// Configure for global error
    /// </summary>
    public class ErrorDetails
    {
        public string? ErrorType { get; set; }
        public int? StatusCode { get; set; }
        public string? Message { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
