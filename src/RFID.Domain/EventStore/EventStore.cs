using RFID.Token.Domain.EventStore.Interfaces;
using System.Collections.Concurrent;

namespace RFID.Token.Domain.EventStore;

/// <summary>
/// Event store implementation. Very basic and lacks a lot of functionality to be a real event store.
/// </summary>
public class EventStore : IEventStore
{
	private readonly ConcurrentDictionary<Guid, List<Event>> _eventStore = new ConcurrentDictionary<Guid, List<Event>>();

	public void AppendEvent(Guid aggregateId, Event @event)
	{
		if (!_eventStore.TryGetValue(aggregateId, out List<Event>? value))
		{
			value = new List<Event>();
			_eventStore[aggregateId] = value;
		}

		value.Add(@event);
	}

	public List<Event> GetEvents(Guid aggregateId)
	{
		if (_eventStore.TryGetValue(aggregateId, out List<Event>? value))
		{
			return value;
		}

		return new List<Event>();
	}
}
