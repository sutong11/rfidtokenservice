using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RFID.Token.Infrastructure.AutoMapper;
using RFID.Token.Infrastructure.Data;
using RFID.Token.Infrastructure.Repository;

namespace RFID.Token.Infrastructure.Test;

public class RepositoryShould
{
	private readonly WriteRFIDTokenRepository writeRepository;
	private readonly ILogger<WriteRFIDTokenRepository> fakeLogger;
	private readonly ReadRFIDTokenRepository readRepository;
	private readonly IMapper _mapper;

	public RepositoryShould()
	{
		//initialize inmemory database for testing
		var options = new DbContextOptionsBuilder<RFIDTokenDbContext>()
		  .UseInMemoryDatabase(databaseName: "RFIDTokenDatabaseTest")
		  .Options;
		MapperConfiguration mapConfig = new MapperConfiguration(cfg =>
		{
			cfg.AddProfile<InfrastructureAutoMappingProfile>();
		});
		_mapper = mapConfig.CreateMapper();

		var context = new RFIDTokenDbContext(options);
		context.Database.EnsureCreated();
		fakeLogger = A.Fake<ILogger<WriteRFIDTokenRepository>>();
		writeRepository = new WriteRFIDTokenRepository(context, _mapper, fakeLogger);
		readRepository = new ReadRFIDTokenRepository(context, _mapper);
	}

	[Fact]
	public void CreateRFIDTokenWithValidInput()
	{
		// Arrange
		var rfId = Guid.NewGuid();
		var customerNumber = Guid.NewGuid();
		var validFrom = DateOnly.Parse("2023-01-06");
		var validTo = DateOnly.Parse("2033-01-07");

		// Act
		var res = writeRepository.CreateRFIDToken(rfId, customerNumber, validFrom, validTo);

		//Assert
		using (new AssertionScope())
		{
			res.Id.Should().Be(rfId);
			res.CustomerNumber.Should().Be(customerNumber);
			res.ValidFrom.Should().Be(validFrom);
			res.ValidTo.Should().Be(validTo);
		}
	}

	[Fact]
	public async Task NotGetRFIDTokenIfIdIsNotFound()
	{
		// Arrange
		var rfId = Guid.NewGuid();

		// Act
		var res = await readRepository.GetRFIDTokenAsync(rfId);

		//Assert
		res.Should().BeNull();
	}

	[Fact]
	public async Task GetRFIDTokenIfItExists()
	{
		// Arrange
		var rfId = Guid.NewGuid();
		var customerNumber = Guid.NewGuid();
		var validFrom = DateOnly.Parse("2023-01-06");
		var validTo = DateOnly.Parse("2033-01-07");

		//this is like testing the create and the read in one ...
		var rftoken = writeRepository.CreateRFIDToken(rfId, customerNumber, validFrom, validTo);

		// Act
		var res = await readRepository.GetRFIDTokenAsync(rftoken.Id);

		//Assert
		using (new AssertionScope())
		{
			res.Should().NotBeNull();
			res!.Id.Should().Be(rftoken.Id);
			res!.CustomerNumber.Should().Be(rftoken.CustomerNumber);
			res!.ValidFrom.Should().Be(rftoken.ValidFrom);
			res!.ValidTo.Should().Be(rftoken.ValidTo);
		}
	}
}