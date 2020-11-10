using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace API.Middleware
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(httpContext, exception, _logger);
            }
        }

        private async Task HandleExceptionAsync(HttpContext httpContext, Exception exception, ILogger<ErrorHandlingMiddleware> logger)
        {
            object errors = null;

            switch (exception)
            {
                case RESTException re:
                    logger.LogError(exception, "REST ERROR");
                    errors = re.Errors;
                    httpContext.Response.StatusCode = (int)re.Code;
                    break;
                case Exception e:
                    logger.LogError(exception, "SERVER ERROR");
                    errors = string.IsNullOrWhiteSpace(e.Message) ? "Error" : e.Message;
                    httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            httpContext.Response.ContentType = "application/json";

            if (errors != null)
            {
                var result = JsonSerializer.Serialize(new { errors });
                await httpContext.Response.WriteAsync(result);
            }
        }
    }
}