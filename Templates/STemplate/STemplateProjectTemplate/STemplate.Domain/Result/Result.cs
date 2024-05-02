using Newtonsoft.Json;
/// <summary>
/// Ardalis Result
/// </summary>
namespace STemplate.Domain.Result
{
    public class Result<T>
    {
        [JsonConstructor]
        public Result(T? data, bool success, string message)
        {
            Data = data;
            Success = success;
            Message = message;
        }
        public Result(T? data, bool success)
        {
            Data = data;
            Success = success;
        }
        public Result(bool success)
        {
            Success = success;
        }
        public Result(bool success, string message)
        {
            Success = success;
            Message = message;
        }
        protected Result()
        {
        }
        public T? Data { get; }
        public bool Success { get; protected set; }
        public string Message { get; protected set; }
    }
}
