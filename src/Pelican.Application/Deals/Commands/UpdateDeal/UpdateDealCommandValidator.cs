using FluentValidation;

namespace Pelican.Application.Deals.Commands.UpdateDeal;

internal sealed class UpdateDealCommandValidator : AbstractValidator<UpdateDealCommand>
{
	public UpdateDealCommandValidator()
	{
		RuleFor(command => command.ObjectId).NotEmpty();
		RuleFor(command => command.PropertyName).NotEmpty();
		RuleFor(command => command.PropertyValue).NotEmpty();
		RuleFor(command => command.SupplierHubSpotId).NotEmpty();
	}
}
