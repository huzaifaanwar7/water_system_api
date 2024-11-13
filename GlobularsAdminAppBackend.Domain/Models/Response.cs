using System;
using System.ComponentModel.DataAnnotations;

namespace GlobularsAdminAppBackend.Domain
{
    public class Response
    {
        public Response(dynamic data, int status = 200, string message = "") {
            Status = status;
            Data = data;
            Message = message;
        }
        public int Status { get; set; }
        public dynamic Data { get; set; }
        public string Message { get; set; }

    }
}


