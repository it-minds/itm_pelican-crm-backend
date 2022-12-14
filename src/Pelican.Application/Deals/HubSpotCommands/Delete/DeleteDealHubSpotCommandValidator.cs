using FluentValidation;

namespace Pelican.Application.Deals.HubSpotCommands.Delete;

internal sealed class DeleteDealHubSpotCommandValidator : AbstractValidator<DeleteDealHubSpotCommand>
{
	public DeleteDealHubSpotCommandValidator()
	{
		RuleFor(command => command.ObjectId).NotEmpty();
	}
}
