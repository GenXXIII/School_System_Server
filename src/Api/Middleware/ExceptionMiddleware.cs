using System.Net;
using FluentValidation;
using Newtonsoft.Json;

namespace Api.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred");
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            HttpStatusCode status = HttpStatusCode.InternalServerError;
            object responseBody;

            switch (exception)
            {
                case ValidationException validationEx:
                    status = HttpStatusCode.BadRequest;

                    responseBody = new
                    {
                        success = false,
                        message = "Validation failed",
                        errors = validationEx.Errors.Select(e => new
                        {
                            field = e.PropertyName,
                            error = e.ErrorMessage
                        })
                    };
                    break;

                case System.ComponentModel.DataAnnotations.ValidationException componentValidationEx:
                    status = HttpStatusCode.BadRequest;

                    responseBody = new
                    {
                        success = false,
                        message = componentValidationEx.Message
                    };
                    break;

                case KeyNotFoundException notFoundEx:
                    status = HttpStatusCode.NotFound;

                    responseBody = new
                    {
                        success = false,
                        message = notFoundEx.Message
                    };
                    break;

                case UnauthorizedAccessException unauthorizedEx:
                    status = HttpStatusCode.Unauthorized;

                    responseBody = new
                    {
                        success = false,
                        message = unauthorizedEx.Message
                    };
                    break;

                default:
                    status = HttpStatusCode.InternalServerError;

                    responseBody = new
                    {
                        success = false,
                        message = "An unexpected error occurred",
                        detail = exception.Message // you can hide this in production
                    };
                    break;
            }

            context.Response.StatusCode = (int)status;
            return context.Response.WriteAsync(
                JsonConvert.SerializeObject(responseBody)
            );
        }
    }
}
