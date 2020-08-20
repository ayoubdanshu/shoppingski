using System;

namespace API.Errors
{
    public class ApiResponse
    {
        public ApiResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatuscode(statusCode);
        }

        public int StatusCode { get; set; }
        public string Message { get; set; }

        private string GetDefaultMessageForStatuscode(int statusCode)
        {
            return statusCode switch
            {
                400 => "A bad Request, you have made",
                401 => "Authorized, you are not",
                404 => "Resource found, it was not",
                500 => "Errors are the path pf the dark side. Errors leads to anger. anger leads to hate. hate leads to career change",
                _ => null
            };


        }

    }
}