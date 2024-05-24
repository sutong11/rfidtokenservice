using MediatR;
using Microsoft.AspNetCore.Mvc;
using RFID.Token.API.Command.Controllers.Create.Models;
using RFID.Token.Application.UseCases;

namespace RFID.Token.API.Command.Controllers.Create;

/// <summary>
/// Create RFIDToken controller
/// </summary>
[ApiController]
public class CreateController : BaseCommandController
{
	private readonly IMediator _mediator;

	public CreateController(IMediator mediator)
	{
		_mediator = mediator;
	}

	//[Authorize]
	[HttpPost("create", Name = "CreateRFIDToken")]
	[ProducesResponseType(StatusCodes.Status202Accepted)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	[ProducesResponseType(StatusCodes.Status422UnprocessableEntity)]
	public async Task<IActionResult> Post([FromBody] CreateRFIDTokenRequestModel @event)
	{
		//get customer number from httpContext
		var customerNumberClaim = HttpContext.User.FindFirst("CustomerNumber");
		if (customerNumberClaim == null)
			return BadRequest("User Claim was not present in HttpContext.");

		var customerNumberValue = customerNumberClaim.Value;

		var parsedCustomerNumber = Guid.TryParse(customerNumberValue, out var customerNumber);
		if (!parsedCustomerNumber)
			return UnprocessableEntity("Customer number is of invalid format.");

		var command = new CreateRFIDTokenCommand(@event.Id, customerNumber, @event.ValidFrom, @event.ValidTo);
		var result = await _mediator.Send(command);
		if (result is false)
			return BadRequest();

		return Accepted();
	}
}
