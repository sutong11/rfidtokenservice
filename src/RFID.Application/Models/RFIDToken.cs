namespace RFID.Token.Application.Models;

/// <summary>
/// RFIDToken Application layer Model
/// </summary>
public class RFIDToken
{
	public required Guid Id { get; set; }
	public required Guid CustomerNumber { get; set; }
	public required DateOnly ValidFrom { get; set; }
	public required DateOnly ValidTo { get; set; }
}
