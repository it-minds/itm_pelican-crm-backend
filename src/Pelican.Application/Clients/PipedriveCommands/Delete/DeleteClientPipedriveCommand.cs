using Pelican.Application.Abstractions.Messaging;

namespace Pelican.Application.Clients.PipedriveCommands.Delete;

public sealed record DeleteClientPipedriveCommand(
	int ClientId) : ICommand;
