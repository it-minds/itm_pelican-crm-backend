using FluentValidation;

namespace Pelican.Application.Contacts.Commands.UpdateContact;

internal sealed class UpdateContactCommandValidator : AbstractValidator<UpdateContactCommand>
{
	public UpdateContactCommandValidator()
	{
		RuleFor(command => command.ObjectId).NotEmpty();
		RuleFor(command => command.PropertyName).NotEmpty();
		RuleFor(command => command.PropertyValue).NotEmpty();
		RuleFor(command => command.PortalId).NotEmpty();
	}
}
