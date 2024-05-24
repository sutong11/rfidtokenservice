using MediatR;

namespace RFID.Token.Domain.Processors.UseCases;

/// <summary>
/// Create RFID Token command. Used to create a new <see cref="RFIDToken"/> in the database.
/// </summary>
/// <param name="RfIdToken"></param>
/// <param name="CustomerNumber"></param>
/// <param name="ValidFrom"></param>
/// <param name="ValidTo"></param>
public record RFIDTokenCreated(Guid RfIdToken, Guid CustomerNumber, DateOnly ValidFrom, DateOnly ValidTo) : INotification;
