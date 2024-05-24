using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RFID.Token.Infrastructure.Models;

namespace RFID.Token.Infrastructure.Data;

/// <summary>
/// Seed data for testing if we wanted to
/// </summary>
public class SeedData
{
	public static void Initialize(IServiceProvider serviceProvider)
	{
		using (var dbContext = new RFIDTokenDbContext(
			serviceProvider.GetRequiredService<DbContextOptions<RFIDTokenDbContext>>()))
		{
			// Check if there are any existing records; if not, seed data
			if (!dbContext.RFIDTokens.Any())
			{
				var token = new RFIDToken
				{
					Id = new Guid("8934fa7b-62a8-4187-b051-b0aa47a55933"), // Generate a new GUID for the token
					CustomerNumber = Guid.NewGuid(), // Assign a unique customer number
					ValidFrom = DateOnly.FromDateTime(DateTime.UtcNow), // Set ValidFrom to current date
					ValidTo = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(30)) // Set ValidTo 30 days from now
				};

				dbContext.RFIDTokens.Add(token);
				dbContext.SaveChanges();
			}
		}
	}
}