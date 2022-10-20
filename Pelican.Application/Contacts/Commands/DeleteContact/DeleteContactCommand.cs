using Pelican.Application.Abstractions.Messaging;

namespace Pelican.Application.Contacts.Commands.DeleteContact;

public sealed record DeleteContactCommand(
	long ObjectId) : ICommand;
