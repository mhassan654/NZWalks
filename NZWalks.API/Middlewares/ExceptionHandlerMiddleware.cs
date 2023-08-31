using System.Net;

namespace NZWalks.API.Middlewares;

public class ExceptionHandlerMiddleware
{
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;
    private readonly RequestDelegate _next;

    public ExceptionHandlerMiddleware(ILogger<ExceptionHandlerMiddleware> logger, RequestDelegate next)
    {
        _logger = logger;
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);

        }
        catch (Exception e)
        {
            var errorId = Guid.NewGuid();
            
            // log this exception
            _logger.LogError(e, $"{errorId} : {e.Message}");
            
            // return a custom error response
            httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            httpContext.Response.ContentType = "application/json";

            var error = new
            {
                Id = errorId,
                ErrorMessage = "Something went wrong! We are looking into resolving this."
            };

            await httpContext.Response.WriteAsJsonAsync(error);
        }
        
    }
}