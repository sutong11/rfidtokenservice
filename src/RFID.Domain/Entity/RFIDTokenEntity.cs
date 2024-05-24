using RFID.Token.Domain.Events;

namespace RFID.Token.Domain.Entity;

/// <summary>
/// Domain Entity, represents a RFID Token.
/// </summary>
public class RFIDTokenEntity
{
	public Guid Id { get; set; }
	public Guid CustomerNumber { get; set; }
	public DateOnly ValidFrom { get; set; }
	public DateOnly ValidTo
	{
		get { return _ValidTo; }
		set { _ = value > ValidFrom ? _ValidTo = value : throw new InvalidDataException("ValidTo needs to be in the future compared to ValidFrom"); }
	}
	private DateOnly _ValidTo { get; set; }
	public void Apply(CreateRFIDTokenEvent createRFIDTokenEvent)
	{
		Id = createRFIDTokenEvent.Id;
		CustomerNumber = createRFIDTokenEvent.CustomerNumber;
		ValidFrom = createRFIDTokenEvent.ValidFrom;
		ValidTo = createRFIDTokenEvent.ValidTo;
	}
}
