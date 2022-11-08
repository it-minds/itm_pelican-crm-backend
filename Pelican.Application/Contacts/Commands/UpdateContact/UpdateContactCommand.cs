namespace Pelican.Application.Contacts.Commands.UpdateContact;
public sealed record UpdateContactCommand(
	long ObjectId,
	long SupplierHubSpotId,
	string PropertyName,
	string PropertyValue) : ICommand;
