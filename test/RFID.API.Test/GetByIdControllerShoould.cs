using RFID.Token.API.Query.Controllers.GetRFIDTokenById;
using RFID.Token.Application.UseCases;

namespace RFID.Token.API.Test;

public class GetByIdControllerShoould
{
	private readonly IMediator _mediator;
	public GetByIdControllerShoould()
	{
		_mediator = A.Fake<IMediator>();
	}


	[Fact]
	public async Task ReturnOkObjectResult()
	{
		// Arrange
		Application.Models.RFIDToken? fakeReturn = new() { Id = Guid.NewGuid(), CustomerNumber = Guid.NewGuid(), ValidFrom = DateOnly.MinValue, ValidTo = DateOnly.MinValue.AddYears(7) };
		var controller = new GetByIdController(_mediator);
		_ = A.CallTo(() => _mediator.Send(A<FindRFIDTokenQuery>._, A<CancellationToken>._)).Returns(Task.FromResult<Application.Models.RFIDToken?>(fakeReturn));
		
		// Act
		var result = await controller.Get(Guid.NewGuid()) as ObjectResult;

		string[] expectedProperties = ["Id", "CustomerNumber", "ValidFrom", "ValidTo"];

		// Assert
		using (new AssertionScope())
		{ 
			result.Should().NotBeNull();
			result!.StatusCode.Should().Be(200);
			result!.Value.Should().NotBeNull();
			result!.Value!.GetType().GetProperty("Id").Should().NotBeNull();
			result!.Value!.GetType().GetProperty("CustomerNumber").Should().NotBeNull();
			result!.Value!.GetType().GetProperty("ValidFrom").Should().NotBeNull();
			result!.Value!.GetType().GetProperty("ValidTo").Should().NotBeNull();
		}

	}

	[Fact]
	public async Task ReturnNotFound()
	{
        // Arrange
        Application.Models.RFIDToken? fakeReturn = null;
		var controller = new GetByIdController(_mediator);
		string[] expectedProperties = ["Id", "CustomerNumber", "ValidFrom", "ValidTo"];

		_ = A.CallTo(() => _mediator.Send(A<FindRFIDTokenQuery>._, A<CancellationToken>._)).Returns(Task.FromResult(fakeReturn));

		// Act
		var result = await controller.Get(Guid.NewGuid()) as NotFoundResult;

		// Assert
		using (new AssertionScope())
		{
			result.Should().NotBeNull();
			result!.StatusCode.Should().Be(404);
		}

	}
}
