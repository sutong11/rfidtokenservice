using AutoMapper;
using RFID.Token.Application.Interfaces;
using RFID.Token.Infrastructure.Data;
using RFID.Token.Infrastructure.Models;

namespace RFID.Token.Infrastructure.Repository;

/// <summary>
/// Repository for reading <see cref="Application.Models.RFIDToken"/> from the database.
/// </summary>
public class ReadRFIDTokenRepository(RFIDTokenDbContext dbContext, IMapper mapper) : IReadRFIDTokenRepository
{
	private readonly RFIDTokenDbContext _context = dbContext;
	private readonly IMapper _mapper = mapper;

	public async Task<Application.Models.RFIDToken?> GetRFIDTokenAsync(Guid rfIdToken)
	{
		var entity = await _context.FindAsync<RFIDToken>(rfIdToken);
		return _mapper.Map<Application.Models.RFIDToken>(entity) ?? null;
	}
}
