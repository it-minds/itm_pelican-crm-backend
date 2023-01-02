using Pelican.Application.Abstractions.Messaging;

namespace Pelican.Application.Contacts.HubSpotCommands.Update;
public sealed record UpdateContactHubSpotCommand(
	long ObjectId,
	long SupplierHubSpotId,
	long UpdateTime,
	string PropertyName,
	string PropertyValue) : ICommand;
