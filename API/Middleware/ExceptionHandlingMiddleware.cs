using API.Models.Errors;
using Application.Exceptions;

namespace API.Middleware;

/// <summary>
/// Provides centralized exception handling for the application.
/// Converts application-level and unexpected exceptions into standardized error responses.
/// </summary>
/// <param name="next">The next middleware delegate in the HTTP request pipeline.</param>
/// <param name="logger">The logger instance to use for logging.</param>
public class ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
{
    /// <summary>
    /// Invokes the middleware logic.
    /// </summary>
    /// <param name="context">The current HTTP context.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (AppException ex)
        {
            logger.LogWarning(ex, "Handled application exception: {Code}", ex.Code);

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = ex.StatusCode;

            var body = new ErrorResponse(ex.Code, ex.Message);
            await context.Response.WriteAsJsonAsync(body);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Unhandled exception occurred");

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var body = new ErrorResponse("INTERNAL_ERROR", "Unexpected server error occurred.");
            await context.Response.WriteAsJsonAsync(body);
        }
    }
}
