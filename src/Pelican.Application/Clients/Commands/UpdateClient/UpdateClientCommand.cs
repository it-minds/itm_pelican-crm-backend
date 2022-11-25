using Pelican.Application.Abstractions.Messaging;

namespace Pelican.Application.Clients.Commands.UpdateClient;
public sealed record UpdateClientCommand(
	long ObjectId,
	long PortalId,
	string PropertyName,
	string PropertyValue) : ICommand;

