using Microsoft.Extensions.Logging;
using RFID.Token.Application.Interfaces;
using RFID.Token.Application.UseCases;
using RFID.Token.Infrastructure.Processors;

namespace RFID.Token.Infrastructure.Test;

public class RFIDTokenCreatedProjectionHandlerShould
{

	private readonly RFIDTokenCreatedProjectionProcessor _handler;
	private readonly IFixture _fixture;
	private readonly IWriteRFIDTokenRepository _writeRFIDTokenRepository;
	private readonly ILogger<RFIDTokenCreatedProjectionProcessor> _logger;
	public RFIDTokenCreatedProjectionHandlerShould()
	{
		_writeRFIDTokenRepository = A.Fake<IWriteRFIDTokenRepository>();
		_fixture = new Fixture().Customize(new CompositeCustomization(new DateOnlyFixtureCustomization()));
		_logger = A.Fake<ILogger<RFIDTokenCreatedProjectionProcessor>>();
		_handler = new RFIDTokenCreatedProjectionProcessor(_writeRFIDTokenRepository, _logger);
	}

	[Fact]
	public void StoreEventAndPublishEvent()
	{
		// Arrange
		var fromDateOnly = DateOnly.Parse("2023-01-06");
		var command = _fixture.Build<RFIDTokenCreatedOccurrence>()
								.With(x => x.ValidFrom, fromDateOnly)
								.With(x => x.ValidTo, fromDateOnly.AddYears(10))
									.Create();

		// Act
		_handler.Handle(command, CancellationToken.None);

		// Assert
		A.CallTo(() => _writeRFIDTokenRepository
							.CreateRFIDToken(command.RfIdToken,command.CustomerNumber,command.ValidFrom,command.ValidTo))
								.MustHaveHappenedOnceExactly();
	}
}