using Pelican.Application.Abstractions.Messaging;

namespace Pelican.Application.Clients.PipedriveCommands.DeleteClient;

public sealed record DeleteClientPipedriveCommand(
	int ClientId) : ICommand;
