using MediatR;
using Microsoft.Extensions.Logging;

namespace RFID.Token.Tools.MediatR.Configurations;

/// <summary>
/// MediatR request behaviour for retries
/// </summary>
/// <typeparam name="TRequest"></typeparam>
/// <typeparam name="TResponse"></typeparam>
public class RequestRetryBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
{
	private readonly ILogger<RequestRetryBehavior<TRequest, TResponse>> _logger;
	private readonly int _maxRetries;
	private readonly int _retryDelayMilliseconds;

	public RequestRetryBehavior(ILogger<RequestRetryBehavior<TRequest, TResponse>> logger, int maxRetries = 3, int retryDelayMilliseconds = 200)
	{
		_logger = logger;
		_maxRetries = maxRetries;
		_retryDelayMilliseconds = retryDelayMilliseconds;
	}

	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		var exceptions = new List<Exception>();

		for (var retry = 0; retry < _maxRetries; retry++)
		{
			try
			{
				if (retry > 0)
				{
					await Task.Delay(_retryDelayMilliseconds, cancellationToken); // Add delay between retries
				}

				var response = await next();
				return response;
			}
			catch (Exception ex)
			{
				exceptions.Add(ex);
				_logger.LogError(ex, $"Retry {retry + 1} failed for {typeof(TRequest).Name}");
			}
		}

		throw new AggregateException($"Retry failed after {_maxRetries} attempts for {typeof(TRequest).Name}", exceptions);
	}
}
