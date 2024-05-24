namespace RFID.Token.Domain.Events;

/// <summary>
/// Create RFID token domain event
/// </summary>
public class CreateRFIDTokenEvent
{
	public required Guid Id { get; set; }
	public required Guid CustomerNumber { get; set; }
	public required DateOnly ValidFrom { get; set; }
	public required DateOnly ValidTo { get; set; }
}
