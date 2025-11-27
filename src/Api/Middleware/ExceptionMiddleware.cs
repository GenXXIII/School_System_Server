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
                await _next(context); // Call next middleware
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";

            HttpStatusCode statusCode = HttpStatusCode.InternalServerError;
            object? response = null;

            switch (exception)
            {
                case ValidationException validationException:
                    statusCode = HttpStatusCode.BadRequest;
                    response = new
                    {
                        error = "Validation failed",
                        details = validationException.Errors.Select(e => new { e.PropertyName, e.ErrorMessage })
                    };
                    break;

                case KeyNotFoundException notFoundException:
                    statusCode = HttpStatusCode.NotFound;
                    response = new { error = notFoundException.Message };
                    break;

                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    response = new { error = exception.Message };
                    break;
            }

            context.Response.StatusCode = (int)statusCode;
            var json = JsonConvert.SerializeObject(response);
            return context.Response.WriteAsync(json);
        }
    }
}
