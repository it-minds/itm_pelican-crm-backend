using FluentValidation;

namespace Pelican.Application.AccountManagers.HubSpotCommands.ValidateWebhookUserId;

internal sealed class ValidateWebhookUserIdCommandValidator : AbstractValidator<ValidateWebhookUserIdCommand>
{
	public ValidateWebhookUserIdCommandValidator()
	{
		RuleFor(command => command.UserId).NotEmpty();
		RuleFor(command => command.SupplierHubSpotId).NotEmpty();
	}
}
