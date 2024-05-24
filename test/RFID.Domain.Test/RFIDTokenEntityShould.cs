using RFID.Token.Domain.Entity;

namespace RFID.Token.Domain.Test;

public class RFIDTokenEntityShould
{
	[Fact]
	public void ThrowErrorIfValidToIsSetToALowerValueThanValidFrom()
	{
		// Arrange
		var domainEntity = new RFIDTokenEntity();
		var fromDateOnly = DateOnly.Parse("2023-01-06");
		domainEntity.ValidFrom = fromDateOnly;

		// Act
		// Assert
		Assert.Throws<InvalidDataException>(() => domainEntity.ValidTo = fromDateOnly);
	}
}