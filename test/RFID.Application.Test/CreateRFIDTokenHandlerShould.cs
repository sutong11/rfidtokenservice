using Microsoft.Extensions.Logging;
using RFID.Token.Application.Handler;
using RFID.Token.Application.UseCases;
using RFID.Token.Domain.EventStore;
using RFID.Token.Domain.EventStore.Interfaces;
using RFID.Token.Domain.Processors.UseCases;
using RFID.Token.Test.Shared;

namespace RFID.Token.Application.Test;

public class CreateRFIDTokenHandlerShould
{
	private readonly IMediator _mediator;
	private readonly ILogger<CreateRFIDTokenHandler> _logger;
	private readonly CreateRFIDTokenHandler _handler;
	private readonly IFixture _fixture;

	public CreateRFIDTokenHandlerShould()
	{
		_mediator = A.Fake<IMediator>();
		_fixture = new Fixture().Customize(new CompositeCustomization(new DateOnlyFixtureCustomization()));
		_logger = A.Fake<ILogger<CreateRFIDTokenHandler>>();
		_handler = new CreateRFIDTokenHandler(_mediator, _logger);
	}

	[Fact]
	public async Task SendAnOccurrenceForCreateRFIDToken()
	{
		// Arrange
		var fromDateOnly = DateOnly.Parse("2023-01-06");
		var command = _fixture.Build<CreateRFIDTokenCommand>()
								.With(x => x.ValidFrom, fromDateOnly)
								.With(x => x.ValidTo, fromDateOnly.AddYears(10))
									.Create();

		// Act
		var result = await _handler.Handle(command, CancellationToken.None);

		// Assert
		A.CallTo(() => _mediator.Send(
							new CreateRFIDTokenOccurrence(command.RfIdToken, command.CustomerNumber, command.ValidFrom, command.ValidTo),
							A<CancellationToken>.Ignored))
								.MustHaveHappenedOnceExactly();
	}
}