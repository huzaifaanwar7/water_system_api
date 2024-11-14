using System.Net;

namespace GlobularsAdmin.Api.Model
{
    public class BaseResponse
    {
        public HttpStatusCode status { get; set; }
        public string message { get; set; }
        public object data { get; set; }
    }
}