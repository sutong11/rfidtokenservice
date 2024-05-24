using RFID.Token.Domain.Events;
using RFID.Token.Domain.EventStore;
using RFID.Token.Test.Shared;

namespace RFID.Token.Domain.Test;

public class EventStoreShould
{
	private readonly EventStore.EventStore _eventStore;
	private readonly IFixture _fixture;

	public EventStoreShould()
	{
		_eventStore = new EventStore.EventStore();
		_fixture = new Fixture().Customize(new CompositeCustomization(new DateOnlyFixtureCustomization()));
	}

	[Fact]
	public void StoreCorrectEventInDomain()
	{
		// Arrange
		var aggregateId = Guid.NewGuid();
		var fromDateOnly = DateOnly.Parse("2023-01-06");
		var createEvent = _fixture.Build<CreateRFIDTokenEvent>()
									.With(x => x.ValidFrom, fromDateOnly)
									.With(x => x.ValidTo, fromDateOnly.AddYears(10)).Create();
		var @event = _fixture.Build<Event>().With(x => x.Data, createEvent).Create();

		// Act
		_eventStore.AppendEvent(aggregateId, @event);

		// Assert
		using (new AssertionScope())
		{
			_eventStore.GetEvents(aggregateId).Should().HaveCount(1);
			_eventStore.GetEvents(aggregateId).First().Data.Should().Be(createEvent);
		}
	}

	[Fact]
	public void ReturnEmptyListIfAggregateIdIsNotInStore()
	{
		// Arrange
		// Act
		// Assert
		_eventStore.GetEvents(Guid.NewGuid()).Should().HaveCount(0);
	}
}
