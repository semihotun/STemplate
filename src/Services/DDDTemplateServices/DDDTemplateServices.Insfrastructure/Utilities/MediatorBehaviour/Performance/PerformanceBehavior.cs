using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
namespace DDDTemplateServices.Insfrastructure.Utilities.MediatorBehaviour.Performance;
/// <summary>
/// Performance pipeline
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public class PerformanceBehavior<TRequest, TResponse>(ILogger<PerformanceBehavior<TRequest, TResponse>> logger,
    Stopwatch stopwatch) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>, IIntervalRequest
{
    private readonly ILogger<PerformanceBehavior<TRequest, TResponse>> _logger = logger;
    private readonly Stopwatch _stopwatch = stopwatch;
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        TResponse response;
        try
        {
            _stopwatch.Start();
            response = await next();
        }
        finally
        {
            if (_stopwatch.Elapsed.TotalSeconds > request.Interval)
            {
                var error = $"PerformanceError = {request.GetType().Name} {_stopwatch.Elapsed.TotalSeconds}";
                Debug.WriteLine(error);
                _logger.LogInformation(error);
            }
            _stopwatch.Restart();
        }
        return response;
    }
}
