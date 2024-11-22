using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Shared.Behaviors;

public class LoggingBehavior<TRequest, TResponse>(
    ILogger<LoggingBehavior<TRequest, TResponse>> logger
) : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull, IRequest<TResponse>
    where TResponse : notnull
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        logger.LogInformation(
            $"[START] Handle Request: {typeof(TRequest).Name} - Response {typeof(TResponse).Name} - Request Data: {request}"
        );

        var timer = new Stopwatch();
        timer.Start();

        var response = await next();

        timer.Stop();

        var timeTaken = timer.Elapsed;
        if (timeTaken.Seconds > 3)
        {
            logger.LogWarning(
                $"[PERFORMANCE] Handle Request: {typeof(TRequest).Name} - Response {typeof(TResponse).Name} - Elapsed Time: {timeTaken.Milliseconds} ms"
            );
        }

        logger.LogInformation(
            $"[END] Handle Request: {typeof(TRequest).Name} - Response {typeof(TResponse).Name} - Elapsed Time: {timeTaken.Milliseconds} ms"
        );

        return response;
    }
}
