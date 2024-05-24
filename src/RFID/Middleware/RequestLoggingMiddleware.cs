using System.Text;

namespace RFID.Token.API.Middleware;

/// <summary>
/// Middleware to log incoming requests and outgoing responses for tracing and debugging
/// </summary>
public class RequestLoggingMiddleware
{
	private readonly RequestDelegate _next;
	private readonly ILogger<RequestLoggingMiddleware> _logger;

	public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
	{
		_next = next;
		_logger = logger;
	}

	public async Task Invoke(HttpContext context)
	{
		string? requestBody;
		if (context.Request.Method == "GET" || context.Request.Method == "DELETE")
		{
			// For GET and DELETE requests, log query parameters
			requestBody = context.Request.QueryString.Value;
		}
		else
		{
			context.Request.EnableBuffering();
			requestBody = await ReadRequestBody(context.Request);
		}

		_logger.LogInformation($"Incoming request: {context.Request.Method} {context.Request.Path} | Body/Query: {requestBody}");

		try
		{
			await _next(context);

			_logger.LogInformation($"Response: {context.Request.Method} {context.Request.Path} | Status code: {context.Response.StatusCode}");
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, $"Request failed: {context.Request.Method} {context.Request.Path} | Error: {ex.Message}");
			throw;
		}
	}

	private static async Task<string> ReadRequestBody(HttpRequest request)
	{
		request.EnableBuffering();

		using var reader = new StreamReader(request.Body, Encoding.UTF8, detectEncodingFromByteOrderMarks: false, bufferSize: 1024, leaveOpen: true);
		var body = await reader.ReadToEndAsync();

		request.Body.Position = 0; // Reset the stream position for later reading

		return body;
	}
}