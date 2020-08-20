namespace API.Errors
{
    public class apiException : ApiResponse
    {
        public apiException(int statusCode, string message = null, string details = null) : base(statusCode, message)
        {
           Details = details;
        }

        public string Details { get; set; }
    }
}