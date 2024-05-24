using Microsoft.EntityFrameworkCore;
using RFID.Token.Infrastructure.Models;

namespace RFID.Token.Infrastructure.Data;

/// <summary>
/// RFID Token Projection DB Context
/// </summary>
public class RFIDTokenDbContext(DbContextOptions<RFIDTokenDbContext> options) : DbContext(options)
{
	public DbSet<RFIDToken> RFIDTokens { get; set; }

	protected override void OnModelCreating(ModelBuilder modelBuilder)
	{
		base.OnModelCreating(modelBuilder);
		modelBuilder.Entity<RFIDToken>(entity =>
		{
			//table
			entity.HasKey(e => e.Id);
			entity.Property(e => e.CustomerNumber).IsRequired();
			entity.Property(e => e.ValidTo).IsRequired();
			entity.Property(e => e.ValidFrom).IsRequired();

			//indexes
			entity.HasIndex(e => e.CustomerNumber);

			//relations
		});
	}
}
