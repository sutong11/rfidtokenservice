using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace RFID.Token.API.ExceptionFilter;

/// <summary>
/// Custom exception filter.
/// maps InvalidDataException to 422 Unprocessable entity
/// Other errors to 500 internal server error
/// </summary>
public class CustomExceptionFilter : IExceptionFilter
{
	private readonly ILogger<CustomExceptionFilter> _logger;

	public CustomExceptionFilter(ILogger<CustomExceptionFilter> logger)
	{
		_logger = logger;
	}

	public void OnException(ExceptionContext context)
	{
		_logger.LogError(context.Exception, "An unhandled exception occurred.");

		if (context.Exception is InvalidDataException)
		{
			// Data-related error (422 Unprocessable Entity)
			context.Result = new ObjectResult(context.Exception.Message)
			{
				StatusCode = 422
			};
		}
		else
		{
			// Other exceptions (500 Internal Server Error)
			context.Result = new ObjectResult("Internal server error")
			{
				StatusCode = 500
			};
		}

		context.ExceptionHandled = true;
	}
}