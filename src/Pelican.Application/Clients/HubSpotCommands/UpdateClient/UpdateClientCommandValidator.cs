using FluentValidation;

namespace Pelican.Application.Clients.HubSpotCommands.UpdateClient;

internal sealed class UpdateClientCommandValidator : AbstractValidator<UpdateClientCommand>
{
	public UpdateClientCommandValidator()
	{
		RuleFor(command => command.ObjectId).NotEmpty();
		RuleFor(command => command.PropertyName).NotEmpty();
		RuleFor(command => command.PropertyValue).NotEmpty();
		RuleFor(command => command.PortalId).NotEmpty();
	}
}
