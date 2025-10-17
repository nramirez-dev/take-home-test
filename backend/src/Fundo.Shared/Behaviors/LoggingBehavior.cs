using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Fundo.Shared.Behaviors;

public class LoggingBehavior
{
    private readonly ILogger<LoggingBehavior> _logger;

    public LoggingBehavior(ILogger<LoggingBehavior> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> ExecuteAsync<TRequest, TResponse>(
        TRequest request,
        Func<Task<TResponse>> action,
        string? operationName = null)
    {
        var requestName = typeof(TRequest).Name;
        var responseName = typeof(TResponse).Name;
        operationName ??= requestName;

        _logger.LogInformation("[START] {Operation} | Request={RequestName} | Data={@Request}",
            operationName, requestName, request);

        var timer = Stopwatch.StartNew();

        try
        {
            var response = await action();

            timer.Stop();

            if (timer.Elapsed.TotalSeconds > 3)
            {
                _logger.LogWarning("[PERFORMANCE] {Operation} took {Elapsed:N2}s", operationName,
                    timer.Elapsed.TotalSeconds);
            }

            _logger.LogInformation("[END] {Operation} | Response={ResponseName} | Duration={Elapsed:N2}s",
                operationName, responseName, timer.Elapsed.TotalSeconds);

            return response;
        }
        catch (Exception ex)
        {
            timer.Stop();
            _logger.LogError(ex, "[ERROR] {Operation} failed after {Elapsed:N2}s", operationName,
                timer.Elapsed.TotalSeconds);
            throw;
        }
    }
}