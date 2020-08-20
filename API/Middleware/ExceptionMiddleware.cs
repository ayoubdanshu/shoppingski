using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using API.Errors;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IHostEnvironment _env;
        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger,

        IHostEnvironment env)
        {
            _env = env;
            _logger = logger;
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var Response = _env.IsDevelopment()
                     ? new apiException((int)HttpStatusCode.InternalServerError, ex.Message,
                      ex.StackTrace.ToString())
                      : new apiException((int)HttpStatusCode.InternalServerError);

                var Options = new JsonSerializerOptions{PropertyNamingPolicy = 
                JsonNamingPolicy.CamelCase};

                var json = JsonSerializer.Serialize(Response, Options);

                await context.Response.WriteAsync(json);
            }
        }
    }
}