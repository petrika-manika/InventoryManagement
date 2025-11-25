using System.Net;
using System.Text.Json;
using FluentValidation;
using InventoryManagement.Domain.Exceptions;

namespace InventoryManagement.API.Middleware;

/// <summary>
/// Global exception handling middleware for consistent error responses.
/// Catches all unhandled exceptions and converts them to appropriate HTTP responses.
/// </summary>
public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlingMiddleware> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExceptionHandlingMiddleware"/> class.
    /// </summary>
    /// <param name="next">The next middleware in the pipeline.</param>
    /// <param name="logger">The logger for logging exceptions.</param>
    public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    /// <summary>
    /// Invokes the middleware to handle exceptions.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "An unhandled exception occurred: {Message}", exception.Message);
            await HandleExceptionAsync(context, exception);
        }
    }

    /// <summary>
    /// Handles the exception and writes an appropriate response.
    /// </summary>
    /// <param name="context">The HTTP context.</param>
    /// <param name="exception">The exception that occurred.</param>
    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";

        var (statusCode, message, errors) = exception switch
        {
            ValidationException validationException => (
                HttpStatusCode.BadRequest,
                "Validation failed",
                validationException.Errors.Select(e => new
                {
                    property = e.PropertyName,
                    error = e.ErrorMessage
                }).ToList<object>()),

            // User Management Exceptions
            UserNotFoundException userNotFoundException => (
                HttpStatusCode.NotFound,
                userNotFoundException.Message,
                null),

            DuplicateEmailException duplicateEmailException => (
                HttpStatusCode.Conflict,
                duplicateEmailException.Message,
                null),

            InvalidCredentialsException invalidCredentialsException => (
                HttpStatusCode.Unauthorized,
                invalidCredentialsException.Message,
                null),

            // Product/Inventory Management Exceptions
            ProductNotFoundException productNotFoundException => (
                HttpStatusCode.NotFound,
                productNotFoundException.Message,
                null),

            DuplicateProductNameException duplicateProductNameException => (
                HttpStatusCode.Conflict,
                duplicateProductNameException.Message,
                null),

            InsufficientStockException insufficientStockException => (
                HttpStatusCode.Conflict,
                insufficientStockException.Message,
                null),

            CannotDeleteProductWithStockException cannotDeleteProductWithStockException => (
                HttpStatusCode.Conflict,
                cannotDeleteProductWithStockException.Message,
                null),

            // Client Management Exceptions
            DuplicateNIPTException duplicateNIPTException => (
                HttpStatusCode.Conflict,
                duplicateNIPTException.Message,
                null),

            InvalidClientDataException invalidClientDataException => (
                HttpStatusCode.BadRequest,
                invalidClientDataException.Message,
                null),

            // General Exceptions
            UnauthorizedAccessException unauthorizedAccessException => (
                HttpStatusCode.Unauthorized,
                unauthorizedAccessException.Message,
                null),

            InvalidOperationException invalidOperationException => (
                HttpStatusCode.BadRequest,
                invalidOperationException.Message,
                null),

            DomainException domainException => (
                HttpStatusCode.BadRequest,
                domainException.Message,
                null),

            _ => (
                HttpStatusCode.InternalServerError,
                "An unexpected error occurred. Please try again later.",
                null)
        };

        context.Response.StatusCode = (int)statusCode;

        var response = new
        {
            statusCode = (int)statusCode,
            message,
            errors
        };

        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };

        var json = JsonSerializer.Serialize(response, options);
        await context.Response.WriteAsync(json);
    }
}
