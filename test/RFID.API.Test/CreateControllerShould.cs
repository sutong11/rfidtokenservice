using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging.Abstractions;
using RFID.Token.API.Command.Controllers.Create;
using RFID.Token.API.Command.Controllers.Create.Models;
using RFID.Token.Application.UseCases;
using System.Security.Claims;

namespace RFID.Token.API.Test;

public class CreateControllerShould
{
	private readonly IMediator _mediator;
	private readonly NullLogger<CreateController> _logger;
	public CreateControllerShould()
    {
        _mediator = A.Fake<IMediator>();
		_logger = new NullLogger<CreateController>();
    }

    [Fact]
	public async Task ContactHandlerIfValidRequest()
	{
		// Arrange
		var controller = new CreateController(_mediator);
		var customerNumber = Guid.NewGuid();
		// Add customerNumber to claims for httpContext
		var identity = new ClaimsIdentity(new[]
		{
			new Claim("CustomerNumber", customerNumber.ToString()),
		}, "TestAuthentication"); ;

		var principal = new ClaimsPrincipal(identity);

		// Set the current principal (user) for the duration of the test
		controller.ControllerContext = new ControllerContext
		{
			HttpContext = new DefaultHttpContext { User = principal }
		};

		A.CallTo(() => _mediator.Send(A<CreateRFIDTokenCommand>._, A<CancellationToken>._)).Returns(Task.FromResult(true));

		// Act
		var result = await controller.Post(new CreateRFIDTokenRequestModel() { Id = Guid.NewGuid(), ValidFrom = DateOnly.MinValue, ValidTo = DateOnly.MinValue.AddYears(2)}) as ObjectResult;

		// Assert
		using (new AssertionScope())
		{ 
			result.Should().NotBeNull();
			result!.StatusCode.Should().Be(202);
			A.CallTo(() => _mediator.Send(A<CreateRFIDTokenCommand>._, A<CancellationToken>._)).MustHaveHappenedOnceExactly();
		}
	}

	[Fact]
	public async Task NotContactHandlerIfNoCustomerNumberAndReturn400()
	{
		// Arrange
		var controller = new CreateController(_mediator);
		var customerNumber = Guid.NewGuid();
		// Add customerNumber to claims for httpContext
		var identity = new ClaimsIdentity(new[]
		{
			new Claim("asdf", customerNumber.ToString()),
		}, "TestAuthentication"); ;

		var principal = new ClaimsPrincipal(identity);

		// Set the current principal (user) for the duration of the test
		controller.ControllerContext = new ControllerContext
		{
			HttpContext = new DefaultHttpContext { User = principal }
		};

		A.CallTo(() => _mediator.Send(A<CreateRFIDTokenCommand>._, A<CancellationToken>._)).Returns(Task.FromResult(true));
		// Act
		var result = await controller.Post(new CreateRFIDTokenRequestModel() { Id = Guid.NewGuid(), ValidFrom = DateOnly.MinValue, ValidTo = DateOnly.MinValue.AddYears(2)}) as ObjectResult;

		// Assert
		using (new AssertionScope())
		{
			result.Should().NotBeNull();
			result!.StatusCode.Should().Be(400);
			A.CallTo(() => _mediator.Send(A<CreateRFIDTokenCommand>._, A<CancellationToken>._)).MustNotHaveHappened();
		}
	}

	[Fact]
	public async Task NotContactHandlerIfNoCustomerNumberAndReturn422()
	{
		// Arrange
		var controller = new CreateController(_mediator);
		// Add customerNumber to claims for httpContext
		var identity = new ClaimsIdentity(new[]
		{
			new Claim("CustomerNumber", "not-gna-work"),
		}, "TestAuthentication"); ;

		var principal = new ClaimsPrincipal(identity);

		// Set the current principal (user) for the duration of the test
		controller.ControllerContext = new ControllerContext
		{
			HttpContext = new DefaultHttpContext { User = principal }
		};

		A.CallTo(() => _mediator.Send(A<CreateRFIDTokenCommand>._, A<CancellationToken>._)).Returns(Task.FromResult(true));
		// Act
		var result = await controller.Post(new CreateRFIDTokenRequestModel() { Id = Guid.NewGuid(), ValidFrom = DateOnly.MinValue, ValidTo = DateOnly.MinValue.AddYears(2) }) as ObjectResult;

		// Assert
		using (new AssertionScope())
		{
			result.Should().NotBeNull();
			result!.StatusCode.Should().Be(422);
			A.CallTo(() => _mediator.Send(A<CreateRFIDTokenCommand>._, A<CancellationToken>._)).MustNotHaveHappened();
		}
	}
}