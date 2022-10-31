using Pelican.Application.Abstractions.Messaging;

namespace Pelican.Application.Contacts.Commands.UpdateContact;
public sealed record UpdateContactCommand(
	long ObjectId,
	string PortalId,
	string PropertyName,
	string PropertyValue) : ICommand;
