namespace RFID.Token.Domain.EventStore;

/// <summary>
/// Event to store in the <see cref="EventStore"/>
/// </summary>
public class Event
{
	public Guid Id { get; set; }
	public string Type { get; set; }
	public DateTimeOffset Timestamp { get; set; }
	public object Data { get; set; }

	public Event(string type, object data)
	{
		Id = Guid.NewGuid();
		Type = type;
		Timestamp = DateTimeOffset.UtcNow;
		Data = data;
	}
}
