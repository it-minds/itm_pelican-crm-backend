using FluentValidation;

namespace Pelican.Application.Contacts.Commands.DeleteContact;

internal sealed class DeleteContactCommandValidator : AbstractValidator<DeleteContactCommand>
{
	public DeleteContactCommandValidator()
	{
		RuleFor(command => command.ObjectId).NotEmpty();
	}
}
