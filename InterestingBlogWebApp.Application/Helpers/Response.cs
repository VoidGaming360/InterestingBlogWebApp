using System.Collections.Generic;
using System.Net;

namespace InterestingBlogWebApp.Application.Helpers
{
    public class Response
    {
        public Response(object? data, List<string>? errors = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            StatusCode = statusCode;
            Data = data;
            Errors = errors == null ? new List<string>() : errors;
        }
        public HttpStatusCode StatusCode { get; set; }
        public object Data { get; set; }
        public List<string> Errors { get; set; }
    }
}
