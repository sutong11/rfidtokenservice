using FluentValidation;

namespace RFID.Token.API.Command.Controllers.Create.Models
{
	/// <summary>
	/// Create RFIDToken request model validator using fluent validation
	/// </summary>
	public class CreateRFIDTokenRequestModelValidator : AbstractValidator<CreateRFIDTokenRequestModel>
	{
		public CreateRFIDTokenRequestModelValidator()
		{
			RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required.");

			RuleFor(x => x.ValidFrom)
				.NotEmpty().WithMessage("ValidFrom is required.");

			RuleFor(x => x.ValidTo)
				.NotEmpty().WithMessage("ValidTo is required.")
				.GreaterThan(x => x.ValidFrom).WithMessage("ValidTo must be greater than ValidFrom.");
		}
	}
}
