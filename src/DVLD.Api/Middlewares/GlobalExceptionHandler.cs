using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace DVLD.Api.Middlewares;

/// <summary>
/// Handles exceptions centrally and generates RFC 7807 compliant ProblemDetails responses.
/// Maps specific domain exceptions to corresponding HTTP status codes with customized messages.
/// </summary>
public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="GlobalExceptionHandler"/> class.
    /// </summary>
    /// <param name="logger">The structured logging provider.</param>
    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// Attempts to handle the unhandled exception asynchronously and write a structured error response.
    /// </summary>
    /// <param name="httpContext">The HTTP context for the executing request.</param>
    /// <param name="exception">The captured unhandled exception.</param>
    /// <param name="cancellationToken">A cancellation token for the asynchronous operation.</param>
    /// <returns>
    /// A value task representing the asynchronous operation, returning <c>true</c> if handled successfully.
    /// </returns>
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        // 1. Record structured log entry in Seq with full stack trace and request path
        _logger.LogError(
            exception,
            "Exception occurred on {Path}: {Message}",
            httpContext.Request.Path,
            exception.Message);

        // 2. Map exception types to specific HTTP status codes, titles, and custom client messages
        var (statusCode, title, detail) = exception switch
        {
            // Case 1: Resource not found - returns 404 with the specific entity detail message
            KeyNotFoundException notFoundEx => (
                StatusCodes.Status404NotFound,
                "Resource Not Found",
                notFoundEx.Message
            ),

            // Case 2: Validation or illegal argument error - returns 400 with the validation message
            ArgumentException argEx => (
                StatusCodes.Status400BadRequest,
                "Invalid Argument",
                argEx.Message
            ),

            // Case 3: Unauthorized access attempt - returns 401 with standard security text
            UnauthorizedAccessException => (
                StatusCodes.Status401Unauthorized,
                "Unauthorized",
                "You do not have permission to access this resource."
            ),

            // Case 4: Database query or downstream dependency timeout - returns 504
            TimeoutException => (
                StatusCodes.Status504GatewayTimeout,
                "Request Timeout",
                "The operation timed out. Please verify connectivity and try again."
            ),

            // Default Case: Unexpected internal server error - masks technical internals from the client
            _ => (
                StatusCodes.Status500InternalServerError,
                "Internal Server Error",
                "An unexpected server error occurred. Please contact support if the issue persists."
            )
        };

        // 3. Assemble the standardized ProblemDetails payload
        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = httpContext.Request.Path
        };

        // 4. Set the HTTP response status code
        httpContext.Response.StatusCode = statusCode;

        // 5. Serialize ProblemDetails to JSON and send to the HTTP response stream
        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        // Return true to signal that the exception has been completely handled
        return true;
    }
}