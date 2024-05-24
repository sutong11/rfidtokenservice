using Microsoft.Extensions.Logging;
using RFID.Token.Application.Handler;
using RFID.Token.Application.Interfaces;
using RFID.Token.Application.UseCases;
using RFID.Token.Domain.Processors.UseCases;
using RFID.Token.Test.Shared;

namespace RFID.Token.Application.Test;

public class RFIDTokenCreatedHandlerShould
{
	private readonly RFIDTokenCreatedHandler _handler;
	private readonly ILogger<RFIDTokenCreatedHandler> _logger;
	private readonly IMediator _mediator;
	private readonly IFixture _fixture;

	public RFIDTokenCreatedHandlerShould()
	{
		_mediator = A.Fake<IMediator>();
		_fixture = new Fixture().Customize(new CompositeCustomization(new DateOnlyFixtureCustomization()));
		_logger = A.Fake<ILogger<RFIDTokenCreatedHandler>>();
		_handler = new RFIDTokenCreatedHandler(_mediator, _logger);
	}

	[Fact]
	public async Task SendAnOccurrenceForRFIDTokenCreated()
	{
		// Arrange
		var fromDateOnly = DateOnly.Parse("2023-01-06");
		var command = _fixture.Build<RFIDTokenCreated>()
								.With(x => x.ValidFrom, fromDateOnly)
								.With(x => x.ValidTo, fromDateOnly.AddYears(10))
									.Create();

		// Act
		await _handler.Handle(command, CancellationToken.None);

		// Assert
		A.CallTo(() => _mediator.Publish(
							new RFIDTokenCreatedOccurrence(command.RfIdToken, command.CustomerNumber, command.ValidFrom, command.ValidTo),
							A<CancellationToken>.Ignored))
								.MustHaveHappenedOnceExactly();
	}
}