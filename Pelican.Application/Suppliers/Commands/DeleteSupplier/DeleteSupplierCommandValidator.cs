using FluentValidation;

namespace Pelican.Application.Suppliers.Commands.DeleteSupplier;

internal sealed class DeleteSupplierCommandValidator : AbstractValidator<DeleteSupplierCommand>
{
	public DeleteSupplierCommandValidator()
	{
		RuleFor(command => command.ObjectId).NotEmpty();
	}
}
