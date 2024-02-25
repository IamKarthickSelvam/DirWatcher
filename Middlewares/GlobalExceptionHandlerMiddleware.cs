
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace DirWatcher.Middlewares
{
    public class GlobalExceptionHandlerMiddleware : IMiddleware
    {
        private readonly ILogger<GlobalExceptionHandlerMiddleware> _logger;

        public GlobalExceptionHandlerMiddleware(ILogger<GlobalExceptionHandlerMiddleware> logger)
        {
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                context.Response.StatusCode = (int) HttpStatusCode.InternalServerError;

                ProblemDetails problem = new()
                {
                    Status = (int) HttpStatusCode.InternalServerError,
                    Type = "Internal Server Error handled at Global Exception Handler",
                    Title = "Internal Server Error",
                    Detail = "An unexpected internal server error has occured, please try again!"
                };

                string json = JsonSerializer.Serialize(problem);

                context.Response.ContentType = "application/json";
            
                await context.Response.WriteAsync(json);
            }
        }
    }
}
