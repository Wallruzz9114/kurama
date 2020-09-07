using System;
using System.Net;

namespace Core.Errors
{
    public class RESTException : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public object Errors { get; }

        public RESTException(HttpStatusCode statusCode, object errors = null)
        {
            StatusCode = statusCode;
            Errors = errors;
        }
    }
}