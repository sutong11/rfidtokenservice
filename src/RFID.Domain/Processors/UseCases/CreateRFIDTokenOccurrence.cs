using MediatR;

namespace RFID.Token.Domain.Processors.UseCases;
/// <summary>
/// Occurence for action Create RFID token for the domain layer.
/// </summary>
/// <param name="RfIdToken"></param>
/// <param name="CustomerNumber"></param>
/// <param name="ValidFrom"></param>
/// <param name="ValidTo"></param>
public record CreateRFIDTokenOccurrence(Guid RfIdToken, Guid CustomerNumber, DateOnly ValidFrom, DateOnly ValidTo) : IRequest<bool>;

