using Pelican.Application.Abstractions.Messaging;

namespace Pelican.Application.Contacts.PipedriveCommands.Delete;

public sealed record DeleteContactPipedriveCommand(
	int ContactId) : ICommand;
