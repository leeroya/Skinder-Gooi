using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Gooi.Core.Infrastructure;

public class RequestPerformanceBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : IRequest<TResponse>
{

  private readonly Stopwatch _timer;
  private readonly ILogger<RequestPerformanceBehaviour<TRequest, TResponse>> _logger;

  public RequestPerformanceBehaviour(ILogger<RequestPerformanceBehaviour<TRequest, TResponse>> logger)
  {
    _timer = new Stopwatch();
    _logger = logger;
  }

  public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
  {
    var requestName = typeof(TRequest).Name;

    _timer.Start();

    var response = await next();

    _timer.Stop();

    if (_timer.ElapsedMilliseconds > 5000)
    {
      _logger.LogWarning("RequestPerformanceBehaviour[{RequestName}]: Long Running Request ({ElapsedMilliseconds} milliseconds)"
        , requestName, _timer.ElapsedMilliseconds);
    }

    return response;
  }
}
