using FluentValidation;

namespace Pelican.Application.HubSpot.Commands.NewInstallation;

public class NewInstallationCommandValidator : AbstractValidator<NewInstallationCommand>
{
	public NewInstallationCommandValidator()
	{
		RuleFor(c => c.Code).NotEmpty();
	}
}
