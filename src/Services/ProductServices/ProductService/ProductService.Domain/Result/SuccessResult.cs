namespace ProductService.Domain.Result
{
    /// <summary>
    /// success command result
    /// </summary>
    public class SuccessResult : Result
    {
        public SuccessResult(string message)
            : base(true, message)
        {
        }
        public SuccessResult()
            : base(true)
        {
        }
    }
}
