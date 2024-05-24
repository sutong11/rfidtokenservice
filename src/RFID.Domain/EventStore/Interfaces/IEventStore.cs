namespace RFID.Token.Domain.EventStore.Interfaces;

/// <summary>
/// Event Store interface
/// </summary>
public interface IEventStore
{
	/// <summary>
	/// Append an event to the event store with the aggregate root id
	/// </summary>
	/// <param name="aggregateId"></param>
	/// <param name="event"></param>
	public void AppendEvent(Guid aggregateId, Event @event);
	/// <summary>
	/// Gets event givens the aggregate root id
	/// </summary>
	/// <param name="aggregateId"></param>
	/// <returns></returns>
	public List<Event> GetEvents(Guid aggregateId);
}