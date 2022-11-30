using FluentValidation;

namespace Pelican.Application.Deals.HubSpotCommands.UpdateDeal;
internal sealed class UpdateDealHubSpotCommandValidator : AbstractValidator<UpdateDealHubSpotCommand>
{
	public UpdateDealHubSpotCommandValidator()
	{
		RuleFor(command => command.ObjectId).NotEmpty();
		RuleFor(command => command.SupplierHubSpotId).NotEmpty();
		RuleFor(command => command.PropertyName).NotEmpty();
		RuleFor(command => command.PropertyValue).NotEmpty();
		RuleFor(command => command.SupplierHubSpotId).NotEmpty();
	}
}
