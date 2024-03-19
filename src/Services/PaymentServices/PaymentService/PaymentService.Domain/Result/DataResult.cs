using Newtonsoft.Json;

namespace PaymentService.Domain.Result
{
    /// <summary>
    /// Query result
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DataResult<T> : Result
    {
        [JsonConstructor]
        public DataResult(T? data, bool success, string message)
            : base(success, message)
        {
            Data = data;
        }
        public DataResult(T? data, bool success)
            : base(success)
        {
            Data = data;
        }
        [JsonProperty]
        public T? Data { get; }
    }
}
