using Newtonsoft.Json;
namespace AdminIdentityService.Insfrastructure.Utilities.MediatorBehaviour.Validation.Exceptions
{
    /// <summary>
    /// throw validation to middleware
    /// </summary>
    public class ValidationErorDetail
    {
        public string? ErrorType { get; set; }
        public int? StatusCode { get; set; }
        public IEnumerable<ValidationData>? Message { get; set; }
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
