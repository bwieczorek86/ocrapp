using System.Net;

namespace OcrPlugin.App.BlazorClient.Server.Configuration
{
    public class DeveloperExceptionPageMiddleware413Handler
    {
        private readonly RequestDelegate _next;

        public DeveloperExceptionPageMiddleware413Handler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext, IHostEnvironment env)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                HandleExceptionAsync(httpContext, ex);
                if (env.IsDevelopment())
                {
                    throw;
                }
            }
        }

        private static void HandleExceptionAsync(HttpContext context, Exception exception)
        {
            if (exception is BadHttpRequestException badRequestException && badRequestException.Message == "Request body too large.")
            {
                context.Response.StatusCode = (int)HttpStatusCode.RequestEntityTooLarge;
            }
        }
    }
}