using Newtonsoft.Json;
namespace AdminIdentityService.Domain.Result
{
    /// <summary>
    /// global result classes
    /// </summary>
    public class Result
    {
        [JsonConstructor]
        public Result(bool success, string message)
            : this(success)
        {
            Message = message;
        }
        public Result(bool success)
        {
            Success = success;
        }
        public bool Success { get; }
        public string? Message { get; }
    }
}
