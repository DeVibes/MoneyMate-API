using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

namespace AccountyMinAPI.Middlewares;

public class GlobalExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;
    public GlobalExceptionHandlingMiddleware(ILogger<GlobalExceptionHandlingMiddleware> logger) 
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            _logger.LogInformation($"Starting request");
            await next(context); 
        }
        catch (UserException ex)
        {
            _logger.LogError(ex.Message);
            int statusCode = ex is NotFoundException ? (int)HttpStatusCode.NotFound : (int)HttpStatusCode.BadRequest;
            ProblemDetails problem = new()
            {
                Status = statusCode,
                Detail = ex.Message
            };
            
            string json = JsonSerializer.Serialize(problem);
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(json);
        }
        catch (ServerException ex)
        {
            _logger.LogError(ex.Message);
            int statusCode = (int)HttpStatusCode.InternalServerError;
            ProblemDetails problem = new()
            {
                Status = statusCode,
                Detail = ex.Message
            };
            
            string json = JsonSerializer.Serialize(problem);
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(json);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex.Message);
            int statusCode = (int)HttpStatusCode.InternalServerError;
            ProblemDetails problem = new()
            {
                Status = statusCode,
                Detail = "An iternal server has occured"
            };

            string json = JsonSerializer.Serialize(problem);
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";
            await context.Response.WriteAsync(json);
        }
    }
}