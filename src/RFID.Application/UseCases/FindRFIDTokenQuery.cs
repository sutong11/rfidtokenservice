using MediatR;
using RFID.Token.Application.Models;

namespace RFID.Token.Application.UseCases;

/// <summary>
/// Find RFID Token query. Used to retrieve a <see cref="RFIDToken"/> from the database.
/// </summary>
/// <param name="RfIdToken"></param>
public record FindRFIDTokenQuery(Guid RfIdToken) : IRequest<RFIDToken?>;
