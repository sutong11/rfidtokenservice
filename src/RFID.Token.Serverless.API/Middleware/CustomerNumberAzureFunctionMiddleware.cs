using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Middleware;
using RFID.Token.Serverless.API.Shared;

namespace RFID.Token.Serverless.API.Middleware;

/// <summary>
/// Middleware for azure function to set customer number
/// </summary>
public class CustomerNumberAzureFunctionMiddleware : IFunctionsWorkerMiddleware
{
	public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
	{
		// Add or update the customer number
		context.Features.Set(new CustomerNumber());

		await next(context);
	}
}
