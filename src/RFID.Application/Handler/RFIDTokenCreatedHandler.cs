using MediatR;
using Microsoft.Extensions.Logging;
using RFID.Token.Application.Interfaces;
using RFID.Token.Application.UseCases;
using RFID.Token.Domain.Processors.UseCases;

namespace RFID.Token.Application.Handler;

/// <summary>
/// RFID Token created projection handler. Uses the <see cref="IWriteRFIDTokenRepository"/> to add the <see cref="RFIDToken"/> to the database.
/// </summary>
public class RFIDTokenCreatedHandler(IMediator mediator, ILogger<RFIDTokenCreatedHandler> logger) : INotificationHandler<RFIDTokenCreated>
{
	private readonly IMediator _mediator = mediator;
	private readonly ILogger<RFIDTokenCreatedHandler> _logger = logger;

	public Task Handle(RFIDTokenCreated request, CancellationToken cancellationToken)
	{
		try
		{
			var occurrence = new RFIDTokenCreatedOccurrence(request.RfIdToken, request.CustomerNumber, request.ValidFrom, request.ValidTo);
			_ = _mediator.Publish(occurrence, cancellationToken);
			return Task.CompletedTask;
		}
		catch (Exception ex)
		{
			//log exception
			_logger.LogError(ex, $"failed to add RFIDToken to the database", [new { request.RfIdToken, request.CustomerNumber, request.ValidFrom, request.ValidTo}]);
			throw;
		}
	}
}
