using Pelican.Application.Abstractions.Messaging;

namespace Pelican.Application.Clients.Commands.DeleteClient;
public sealed record DeleteClientCommand(
	long ObjectId) : ICommand;
