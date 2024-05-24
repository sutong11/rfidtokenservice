using Microsoft.Extensions.Logging;
using RFID.Token.Domain.EventStore;
using RFID.Token.Domain.EventStore.Interfaces;
using RFID.Token.Domain.Processors.Create;
using RFID.Token.Domain.Processors.UseCases;
using RFID.Token.Test.Shared;

namespace RFID.Token.Domain.Test;

public class CreateRFIDTokenProcessorShould
{
	private readonly IEventStore _eventStore;
	private readonly IMediator _mediator;
	private readonly CreateRFIDTokenProcessor _handler;
	private readonly ILogger<CreateRFIDTokenProcessor> _logger;
	private readonly IFixture _fixture;

	public CreateRFIDTokenProcessorShould()
	{
		_eventStore = A.Fake<IEventStore>();
		_mediator = A.Fake<IMediator>();
		_fixture = new Fixture().Customize(new CompositeCustomization(new DateOnlyFixtureCustomization()));
		_logger = A.Fake<ILogger<CreateRFIDTokenProcessor>>();
		_handler = new CreateRFIDTokenProcessor(_eventStore, _mediator, _logger);
	}

	[Fact]
	public void StoreEventAndPublishEvent()
	{
		// Arrange
		var fromDateOnly = DateOnly.Parse("2023-01-06");
		var command = _fixture.Build<CreateRFIDTokenOccurrence>()
								.With(x => x.ValidFrom, fromDateOnly)
								.With(x => x.ValidTo, fromDateOnly.AddYears(10))
									.Create();

		// Act
		_handler.Handle(command, CancellationToken.None);

		// Assert
		A.CallTo(() => _eventStore.AppendEvent(command.RfIdToken, A<Event>.Ignored)).MustHaveHappenedOnceExactly();
		A.CallTo(() => _mediator.Publish(
							new RFIDTokenCreated(command.RfIdToken, command.CustomerNumber, command.ValidFrom, command.ValidTo),
							A<CancellationToken>.Ignored))
								.MustHaveHappenedOnceExactly();
	}
}