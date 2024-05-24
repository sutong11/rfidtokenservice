using MediatR;
using Microsoft.Extensions.Logging;
using RFID.Token.Application.UseCases;
using RFID.Token.Domain.Processors.UseCases;

namespace RFID.Token.Application.Handler;

/// <summary>
/// MediatR handler for CreateRFIDToken commands.
/// It will create a domain event and persist it to the event store, and publishes an Created event to the mediator.
/// </summary>
public class CreateRFIDTokenHandler(IMediator mediator, ILogger<CreateRFIDTokenHandler> logger) : IRequestHandler<CreateRFIDTokenCommand, bool>
{
	private readonly IMediator _mediator = mediator;
	private readonly ILogger<CreateRFIDTokenHandler> _logger = logger;

	public async Task<bool> Handle(CreateRFIDTokenCommand request, CancellationToken cancellationToken)
	{
		try
		{
			var occurrence = new CreateRFIDTokenOccurrence(request.RfIdToken, request.CustomerNumber, request.ValidFrom, request.ValidTo);
			var result = await _mediator.Send(occurrence, cancellationToken);
			return result;
		}
		catch (Exception ex)
		{
			//log exception
			_logger.LogError(ex, "Failed to process Create RFID Token request", new[] {request});
			return false;
		}
	}
}
