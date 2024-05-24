namespace RFID.Token.API.Command.Controllers.Create.Models
{
	/// <summary>
	/// Create RFID Token Request Model
	/// </summary>
	public record CreateRFIDTokenRequestModel
	{
		public required Guid Id { get; set; }
		public required DateOnly ValidFrom { get; set; }
		public required DateOnly ValidTo { get; set; }
	}
}
