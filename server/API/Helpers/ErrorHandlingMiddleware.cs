using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Core.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace API.Helpers
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _requestDelegate;
        private readonly ILogger<ErrorHandlingMiddleware> _logger;

        public ErrorHandlingMiddleware(RequestDelegate requestDelegate, ILogger<ErrorHandlingMiddleware> logger)
        {
            _logger = logger;
            _requestDelegate = requestDelegate;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _requestDelegate(httpContext);
            }
            catch (Exception exception)
            {
                await HandleExceptionAsync(httpContext, exception, _logger);
            }
        }

        private async Task HandleExceptionAsync(
            HttpContext httpContext,
            Exception exception,
            ILogger<ErrorHandlingMiddleware> logger)
        {
            object errors = null;

            switch (exception)
            {
                case RESTException re:
                    _logger.LogError(exception, "REST ERROR");
                    errors = re.Errors;
                    httpContext.Response.StatusCode = (int)re.StatusCode;
                    break;
                case Exception e:
                    logger.LogError(exception, "SERVER ERROR");
                    errors = string.IsNullOrWhiteSpace(exception.Message) ? "Error" : exception.Message;
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