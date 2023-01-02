using FluentValidation;

namespace Pelican.Application.HubSpot.Commands.NewInstallation;

public class NewInstallationHubSpotCommandValidator : AbstractValidator<NewInstallationHubSpotCommand>
{
	public NewInstallationHubSpotCommandValidator()
	{
		RuleFor(c => c.Code).NotEmpty();
	}
}
