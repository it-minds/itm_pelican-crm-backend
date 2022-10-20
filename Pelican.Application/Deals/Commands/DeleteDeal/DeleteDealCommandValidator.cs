using FluentValidation;

namespace Pelican.Application.Deals.Commands.DeleteDeal;

internal sealed class DeleteDealCommandValidator : AbstractValidator<DeleteDealCommand>
{
	public DeleteDealCommandValidator()
	{
		RuleFor(command => command.ObjectId).NotEmpty();
	}
}
