using System;
using System.Net;

namespace Core.Errors
{
    public class RESTException : Exception
    {
        public RESTException(HttpStatusCode code, object errors = null)
        {
            Code = code;
            Errors = errors;
        }

        public HttpStatusCode Code { get; set; }
        public object Errors { get; }
    }
}