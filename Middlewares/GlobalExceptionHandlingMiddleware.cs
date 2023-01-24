using System.Net;

namespace AccountyMinAPI.Middlewares;

public class GlobalExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;
    public GlobalExceptionHandlingMiddleware(RequestDelegate next, ILogger logger) 
    {
        _next = next;
        _logger = logger;
    }
    public async Task InvoleAsync(HttpContext context)
    {
        try
        {
            await _next(context); 
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        }
    }
}