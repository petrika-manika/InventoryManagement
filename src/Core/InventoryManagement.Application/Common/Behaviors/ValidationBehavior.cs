using FluentValidation;
using MediatR;

namespace InventoryManagement.Application.Common.Behaviors;

/// <summary>
/// MediatR pipeline behavior that automatically validates requests before they reach handlers.
/// This demonstrates the Open/Closed Principle - adding validation without modifying existing handlers.
/// Any request with a registered validator will be validated automatically.
/// If validation fails, a ValidationException is thrown before the handler is invoked.
/// </summary>
/// <typeparam name="TRequest">The type of request being handled.</typeparam>
/// <typeparam name="TResponse">The type of response from the handler.</typeparam>
public sealed class ValidationBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly IEnumerable<IValidator<TRequest>> _validators;

    /// <summary>
    /// Initializes a new instance of the <see cref="ValidationBehavior{TRequest, TResponse}"/> class.
    /// </summary>
    /// <param name="validators">Collection of validators for the request type.</param>
    public ValidationBehavior(IEnumerable<IValidator<TRequest>> validators)
    {
        _validators = validators;
    }

    /// <summary>
    /// Handles the request by validating it before passing to the next handler in the pipeline.
    /// </summary>
    /// <param name="request">The request to validate and handle.</param>
    /// <param name="next">The next handler in the pipeline.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The response from the handler.</returns>
    /// <exception cref="ValidationException">Thrown when validation fails.</exception>
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        // If no validators are registered for this request type, skip validation
        if (!_validators.Any())
        {
            return await next();
        }

        // Create validation context for the request
        var context = new ValidationContext<TRequest>(request);

        // Validate all validators in parallel for better performance
        var validationResults = await Task.WhenAll(
            _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        // Collect all validation failures from all validators
        var failures = validationResults
            .SelectMany(result => result.Errors)
            .Where(failure => failure != null)
            .ToList();

        // If any validation failures exist, throw ValidationException
        if (failures.Any())
        {
            throw new ValidationException(failures);
        }

        // All validations passed, proceed to the next handler in the pipeline
        return await next();
    }
}
