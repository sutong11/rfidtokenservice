using System.Security.Claims;

namespace RFID.Token.API.Middleware;

/// <summary>
/// Customer Number Middleware, used to hardcode the customer number in the user.identity claims.
/// If a proper authentication is implemented, it would substitute this middleware
/// </summary>
public class CustomerNumberMiddleware
{
	private readonly RequestDelegate _next;
	private const string CustomCustomerNumber = "8934fa7b-62a8-4187-b051-b0aa47a55933"; // Your custom customer number

	public CustomerNumberMiddleware(RequestDelegate next)
	{
		_next = next;
	}

	public async Task Invoke(HttpContext context)
	{
		var user = context.User;


		var claimsIdentity = (ClaimsIdentity)user.Identity;

		// Add or update the customer number claim
		claimsIdentity!.AddClaim(new Claim("CustomerNumber", CustomCustomerNumber));


		await _next(context);
	}
}
