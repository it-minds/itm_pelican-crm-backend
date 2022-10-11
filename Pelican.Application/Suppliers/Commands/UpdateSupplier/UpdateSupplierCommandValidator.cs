using FluentValidation;

namespace Pelican.Application.Suppliers.Commands.UpdateSupplier;

internal sealed class UpdateSupplierCommandValidator : AbstractValidator<UpdateSupplierCommand>
{
	public UpdateSupplierCommandValidator()
	{
		RuleFor(command => command.ObjectId).NotEmpty();
		RuleFor(command => command.PropertyName).NotEmpty();
		RuleFor(command => command.PropertyValue).NotEmpty();
		RuleFor(command => command.UserId).NotEmpty();
	}
}
