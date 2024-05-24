using AutoMapper;
using RFID.Token.Application.Handler;
using RFID.Token.Application.Interfaces;
using RFID.Token.Application.UseCases;
using RFID.Token.Test.Shared;

namespace RFID.Token.Application.Test;

public class ReadRFIDTokenQueryHandlerShould
{
	private readonly ReadRFIDTokenQueryHandler _handler;
	private readonly IReadRFIDTokenRepository _readRFIDTokenRepository;
	private readonly IFixture _fixture;

	public ReadRFIDTokenQueryHandlerShould()
	{
		_fixture = new Fixture().Customize(new CompositeCustomization(new DateOnlyFixtureCustomization()));
		_readRFIDTokenRepository = A.Fake<IReadRFIDTokenRepository>();
		_handler = new ReadRFIDTokenQueryHandler(_readRFIDTokenRepository);
	}

	[Fact]
	public async Task CallRepositoryWithTheCorrectData()
	{
		// Arrange
		var rfIdToken = Guid.NewGuid();
		var query = _fixture.Build<FindRFIDTokenQuery>().With(x => x.RfIdToken, rfIdToken).Create();

		// Act
		await _handler.Handle(query, CancellationToken.None);

		// Assert
		A.CallTo(() =>
			_readRFIDTokenRepository
					.GetRFIDTokenAsync(rfIdToken))
						.MustHaveHappenedOnceExactly();
	}
}