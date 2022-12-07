using Pelican.Application.Abstractions.Messaging;

namespace Pelican.Application.Contacts.Commands.UpdateContact;
public sealed record UpdateContactCommand(
	long ObjectId,
	long SupplierHubSpotId,
	long UpdateTime,
	string PropertyName,
	string PropertyValue) : ICommand;
