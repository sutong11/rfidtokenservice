using MediatR;
using Microsoft.Extensions.Logging;
using RFID.Token.Application.Interfaces;
using RFID.Token.Application.UseCases;

namespace RFID.Token.Infrastructure.Processors;

/// <summary>
/// RFID Token created projection handler. Uses the <see cref="IWriteRFIDTokenRepository"/> to add the <see cref="RFIDToken"/> to the database.
/// </summary>
public class RFIDTokenCreatedProjectionProcessor(IWriteRFIDTokenRepository writeRFIDTokenRepository, ILogger<RFIDTokenCreatedProjectionProcessor> logger) : INotificationHandler<RFIDTokenCreatedOccurrence>
{
	private readonly IWriteRFIDTokenRepository _writeRFIDTokenRepository = writeRFIDTokenRepository;
	private readonly ILogger<RFIDTokenCreatedProjectionProcessor> _logger = logger;

	public Task Handle(RFIDTokenCreatedOccurrence request, CancellationToken cancellationToken)
	{
		try
		{
			_writeRFIDTokenRepository.CreateRFIDToken(request.RfIdToken, request.CustomerNumber, request.ValidFrom, request.ValidTo);
			return Task.CompletedTask;
		}
		catch (Exception ex)
		{
			//log exception
			_logger.LogError(ex, $"failed to add RFIDToken to the database", [new { request.RfIdToken, request.CustomerNumber, request.ValidFrom, request.ValidTo }]);
			throw;
		}
	}
}
