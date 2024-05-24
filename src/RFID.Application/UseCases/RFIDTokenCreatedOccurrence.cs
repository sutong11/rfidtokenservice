using MediatR;

namespace RFID.Token.Application.UseCases;

/// <summary>
/// Occurrence to update projection for a RFID Token Created Projection
/// </summary>
/// <param name="RfIdToken"></param>
/// <param name="CustomerNumber"></param>
/// <param name="ValidFrom"></param>
/// <param name="ValidTo"></param>
public record RFIDTokenCreatedOccurrence(Guid RfIdToken, Guid CustomerNumber, DateOnly ValidFrom, DateOnly ValidTo) : INotification;
