using RFID.Token.Application.Models;

namespace RFID.Token.Application.Interfaces;

/// <summary>
/// Interface for writing <see cref="RFIDToken"/> to the database.
/// </summary>
public interface IWriteRFIDTokenRepository
{
	/// <summary>
	/// Creates a RFID token with given information and returns the GUID for the created token
	/// </summary>
	/// <param name="rfIdToken"></param>
	/// <param name="customerNumber"></param>
	/// <param name="validFrom"></param>
	/// <param name="validTo"></param>
	/// <returns></returns>
	public RFIDToken CreateRFIDToken(Guid rfIdToken, Guid customerNumber, DateOnly validFrom, DateOnly validTo);
}
