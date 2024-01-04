using AdminIdentityService.Insfrastructure.Utilities.MediatorBehaviour.Validation.Exceptions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;
using System.Net;
using System.Security;
namespace AdminIdentityService.Insfrastructure.Utilities.Exceptions.GlobalEror
{
    /// <summary>
    /// all eror arrived middleware
    /// </summary>
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception e)
            {
                await HandleExceptionAsync(httpContext, e);
            }
        }
        private async Task HandleExceptionAsync(HttpContext httpContext, Exception e)
        {
            var errorDetails = new ErrorDetails();
            errorDetails.StatusCode = (int)HttpStatusCode.InternalServerError;
            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            if (e.GetType() == typeof(CustomValidatonException))
            {
                var validationErorDetail = new ValidationErorDetail();
                validationErorDetail.Message = JsonConvert.DeserializeObject<IEnumerable<ValidationData>>(e.Message);
                validationErorDetail.StatusCode = (int)HttpStatusCode.BadRequest;
                validationErorDetail.ErrorType = "ValidationException";
                var customValidationObject = JsonConvert.SerializeObject(validationErorDetail);
                Log.Error(customValidationObject);
                await httpContext.Response.WriteAsync(customValidationObject);
                return;
            }
            else if (e.GetType() == typeof(UnauthorizedAccessException))
            {
                errorDetails.Message = e.Message;
                errorDetails.StatusCode = StatusCodes.Status401Unauthorized;
                errorDetails.ErrorType = "UnauthorizedAccessException";
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            }
            else if (e.GetType() == typeof(SecurityException))
            {
                errorDetails.Message = e.Message;
                errorDetails.StatusCode = StatusCodes.Status401Unauthorized;
                errorDetails.ErrorType = "SecurityException";
                httpContext.Response.StatusCode = StatusCodes.Status401Unauthorized;
            }
            else if (e.GetType() == typeof(ApplicationException))
            {
                errorDetails.Message = e.Message;
                errorDetails.StatusCode = (int)HttpStatusCode.BadRequest;
                errorDetails.ErrorType = "ApplicationException";
                httpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
            else
            {
                errorDetails.Message = "Unexpected error";
            }
            var serializeObject = JsonConvert.SerializeObject(errorDetails);
            Log.Error(serializeObject);
            await httpContext.Response.WriteAsync(serializeObject);
        }
    }
}
