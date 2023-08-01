using FluentResults;
using MediatR;

namespace BlazorApp.Behaviors;

/// <summary>
/// A MediatR pipeline behavior that logs each request and response.
/// </summary>
/// <typeparam name="TRequest">
/// Generic argument that represents the type of the request.
/// </typeparam>
/// <typeparam name="TResponse">
/// Generic argument that represents the type of the response.
/// </typeparam>
public sealed class LoggingPipelineBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{
    private readonly ILogger<LoggingPipelineBehavior<TRequest, TResponse>> _logger;

    public LoggingPipelineBehavior(ILogger<LoggingPipelineBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling {@RequestType} on {@DateTimeUtc}", typeof(TRequest).Name, DateTime.UtcNow);

        var response = await next();

        if (response is IResultBase result && result.IsFailed)
        {
            _logger.LogInformation("Errors {@Errors} on {@DateTimeUtc}", result.Errors, DateTime.UtcNow);
        }

        if (response is Exception)
        {
            _logger.LogError("Error {@Exception} on {@DateTimeUtc}", response, DateTime.UtcNow);
        }

        _logger.LogInformation("Handled {@RequestType} on {@DateTimeUtc}", typeof(TRequest).Name, DateTime.UtcNow);

        return response;
    }
}