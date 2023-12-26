﻿namespace DDDTemplateService.Domain.Result
{
    /// <summary>
    /// error command result
    /// </summary>
    public class ErrorResult : Result
    {
        public ErrorResult(string message)
            : base(false, message)
        {
        }
        public ErrorResult()
            : base(false)
        {
        }
    }
}
