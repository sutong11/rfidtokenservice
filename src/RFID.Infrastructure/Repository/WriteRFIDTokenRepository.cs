using AutoMapper;
using Microsoft.Extensions.Logging;
using RFID.Token.Application.Interfaces;
using RFID.Token.Infrastructure.Data;
using RFID.Token.Infrastructure.Models;

namespace RFID.Token.Infrastructure.Repository;

/// <summary>
/// Write RFID Token repository. Uses the <see cref="RFIDTokenDbContext"/> to write the <see cref="RFIDToken"/> to the database.
/// </summary>
public class WriteRFIDTokenRepository(RFIDTokenDbContext dbContext, IMapper mapper, ILogger<WriteRFIDTokenRepository> logger) : IWriteRFIDTokenRepository
{
	private readonly RFIDTokenDbContext _context = dbContext;
	private readonly ILogger<WriteRFIDTokenRepository> _logger = logger;
	private readonly IMapper _mapper = mapper;

	public Application.Models.RFIDToken CreateRFIDToken(Guid rfIdToken, Guid customerNumber, DateOnly validFrom, DateOnly validTo)
	{
		try
		{
			var entity = _context.Add(new RFIDToken()
			{
				Id = rfIdToken,
				CustomerNumber = customerNumber,
				ValidFrom = validFrom,
				ValidTo = validTo
			});
			_context.SaveChanges();
			return _mapper.Map<Application.Models.RFIDToken>(entity.Entity);
		}
		catch (Exception ex)
		{
			//log exception
			_logger.LogError(ex, $"failed to add RFIDToken to the database", [new { rfIdToken, customerNumber, validFrom, validTo}]);
			throw;
		}
	}
}
