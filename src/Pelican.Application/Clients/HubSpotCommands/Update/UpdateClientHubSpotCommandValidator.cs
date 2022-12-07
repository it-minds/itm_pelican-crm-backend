using FluentValidation;

namespace Pelican.Application.Clients.HubSpotCommands.UpdateClient;

internal sealed class UpdateClientHubSpotCommandValidator : AbstractValidator<UpdateClientHubSpotCommand>
{
	public UpdateClientHubSpotCommandValidator()
	{
		RuleFor(command => command.ObjectId).NotEmpty();
		RuleFor(command => command.PropertyName).NotEmpty();
		RuleFor(command => command.PropertyValue).NotEmpty();
		RuleFor(command => command.PortalId).NotEmpty();
	}
}
