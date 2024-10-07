using System.Net;

namespace PrescottAppBackend.Api.Model
{
    public class BaseResponse
    {
        public HttpStatusCode status { get; set; }
        public string message { get; set; }
        public object data { get; set; }
    }
}