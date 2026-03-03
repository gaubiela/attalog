using System.Text.Json;

namespace AttaLog.Api.Middleware;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionMiddleware> _logger;

    public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Bad request: {Message}", ex.Message);
            await WriteResponseAsync(context, StatusCodes.Status400BadRequest, ex.Message);
        }
        catch (UnauthorizedAccessException ex)
        {
            _logger.LogWarning(ex, "Unauthorized access attempt");
            await WriteResponseAsync(context, StatusCodes.Status401Unauthorized, "Unauthorized");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception");
            await WriteResponseAsync(context, StatusCodes.Status500InternalServerError, "An unexpected error occurred");
        }
    }

    private static async Task WriteResponseAsync(HttpContext context, int statusCode, string message)
    {
        context.Response.StatusCode = statusCode;
        context.Response.ContentType = "application/json";

        var json = JsonSerializer.Serialize(new { message });
        await context.Response.WriteAsync(json);
    }
}
