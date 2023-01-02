using FluentValidation;

namespace Pelican.Application.Contacts.HubSpotCommands.Update;

internal sealed class UpdateContactHubSpotCommandValidator : AbstractValidator<UpdateContactHubSpotCommand>
{
	public UpdateContactHubSpotCommandValidator()
	{
		RuleFor(command => command.ObjectId).NotEmpty();
		RuleFor(command => command.SupplierHubSpotId).NotEmpty();
		RuleFor(command => command.PropertyName).NotEmpty();
		RuleFor(command => command.PropertyValue).NotEmpty();
		RuleFor(command => command.UpdateTime).NotEmpty();
	}
}
