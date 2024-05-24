using MediatR;
using RFID.Token.Application.Interfaces;
using RFID.Token.Application.Models;
using RFID.Token.Application.UseCases;

namespace RFID.Token.Application.Handler;

/// <summary>
/// Read RFID Token query handler. Uses the <see cref="IReadRFIDTokenRepository"/> to retrieve the <see cref="RFIDToken"/> from the database.
/// </summary>
public class ReadRFIDTokenQueryHandler(IReadRFIDTokenRepository readRFIDTokenRepository) : IRequestHandler<FindRFIDTokenQuery, RFIDToken?>
{
	private readonly IReadRFIDTokenRepository _readRFIDTokenRepository = readRFIDTokenRepository;

	public async Task<RFIDToken?> Handle(FindRFIDTokenQuery request, CancellationToken cancellationToken)
	{
		return await _readRFIDTokenRepository.GetRFIDTokenAsync(request.RfIdToken);
	}
}
