using Pelican.Application.Abstractions.Messaging;

namespace Pelican.Application.Clients.Commands.UpdateClient;
public sealed record UpdateClientCommand(
	long ObjectId,
	string UserId,
	string PropertyName,
	string PropertyValue) : ICommand;

