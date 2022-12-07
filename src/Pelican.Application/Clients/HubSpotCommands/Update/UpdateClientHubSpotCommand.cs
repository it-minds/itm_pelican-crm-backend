using Pelican.Application.Abstractions.Messaging;

namespace Pelican.Application.Clients.HubSpotCommands.UpdateClient;
public sealed record UpdateClientHubSpotCommand(
	long ObjectId,
	long PortalId,
	long UpdateTime,
	string PropertyName,
	string PropertyValue) : ICommand;

