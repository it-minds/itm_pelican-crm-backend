using Pelican.Application.Abstractions.Messaging;

namespace Pelican.Application.Clients.HubSpotCommands.DeleteClient;
public sealed record DeleteClientCommand(
	long ObjectId) : ICommand;
