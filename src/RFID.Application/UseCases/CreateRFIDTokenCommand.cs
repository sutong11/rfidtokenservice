using MediatR;

namespace RFID.Token.Application.UseCases;

/// <summary>
/// MediatR command to create a new RFID Token.
/// </summary>
/// <param name="RfIdToken"></param>
/// <param name="CustomerNumber"></param>
/// <param name="ValidFrom"></param>
/// <param name="ValidTo"></param>
public record CreateRFIDTokenCommand(Guid RfIdToken, Guid CustomerNumber, DateOnly ValidFrom, DateOnly ValidTo) : IRequest<bool>;
