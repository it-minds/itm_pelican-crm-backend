using Pelican.Application.Abstractions.Messaging;

namespace Pelican.Application.AccountManagers.HubSpotCommands.ValidateWebhookUserId;

public sealed record ValidateWebhookUserIdCommand(
	long UserId,
	long SupplierHubSpotId) : ICommand;
