using Newtonsoft.Json;

namespace BasketService.Domain.Result
{
    /// <summary>
    /// success query result
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SuccessDataResult<T> : DataResult<T>
    {
        [JsonConstructor]
        public SuccessDataResult(T? data, string message)
            : base(data, true, message)
        {
        }
        public SuccessDataResult(T? data)
            : base(data, true)
        {
        }
        public SuccessDataResult(string message)
            : base(default, true, message)
        {
        }
        public SuccessDataResult()
            : base(default, true)
        {
        }
    }
}
