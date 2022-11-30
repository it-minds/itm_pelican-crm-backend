using FluentValidation;

namespace Pelican.Application.Deals.HubSpotCommands.DeleteDeal;

internal sealed class DeleteDealHubSpotCommandValidator : AbstractValidator<DeleteDealHubSpotCommand>
{
	public DeleteDealHubSpotCommandValidator()
	{
		RuleFor(command => command.ObjectId).NotEmpty();
	}
}
