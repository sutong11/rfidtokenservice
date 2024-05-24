using RFID.Token.Application.Models;

namespace RFID.Token.Application.Interfaces;

/// <summary>
/// Interface for reading <see cref="RFIDToken"/> from the database.
/// </summary>
public interface IReadRFIDTokenRepository
{
	/// <summary>
	/// Get a <see cref="RFIDToken"/> from the database.
	/// </summary>
	/// <param name="rfIdToken"></param>
	/// <returns></returns>
	public Task<RFIDToken?> GetRFIDTokenAsync(Guid rfIdToken);
}
