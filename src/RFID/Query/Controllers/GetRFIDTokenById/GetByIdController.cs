using MediatR;
using Microsoft.AspNetCore.Mvc;
using RFID.Token.Application.UseCases;

namespace RFID.Token.API.Query.Controllers.GetRFIDTokenById;

/// <summary>
/// Get RFIDToken by Id controller
/// </summary>
public class GetByIdController : BaseQueryController
{
	private readonly IMediator _mediator;
        public GetByIdController(IMediator mediator)
        {
            _mediator = mediator;
        }

	//[Authorize]
	[HttpGet("get-by-id", Name = "Get By Id")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status404NotFound)]
	public async Task<IActionResult> Get([FromQuery]Guid RFIDTokenId)
	{
		var rfidToken = await _mediator.Send(new FindRFIDTokenQuery(RFIDTokenId));
		if (rfidToken == null)
		{
			return NotFound();
		}
		var response = new
		{
			rfidToken.Id,
			rfidToken.CustomerNumber,
			rfidToken.ValidFrom,
			rfidToken.ValidTo
		};
		return Ok(response);
	}
}
