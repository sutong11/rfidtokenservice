namespace RFID.Token.Infrastructure.Models;

/// <summary>
/// EF Core entity model for the RFIDToken table.
/// </summary>
public class RFIDToken
{
	public Guid Id { get; set; }
	public Guid CustomerNumber { get; set; }
	public DateOnly ValidFrom { get; set; }
	public DateOnly ValidTo { get; set; }
}
