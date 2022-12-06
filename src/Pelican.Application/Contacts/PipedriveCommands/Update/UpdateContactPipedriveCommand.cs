using Pelican.Application.Abstractions.Messaging;

namespace Pelican.Application.Contacts.PipedriveCommands.Update;
public sealed record UpdateContactPipedriveCommand(
	int supplierPipedriveId,
	int contactId,
	int contactPipedriveOwnerId,
	string? firstName,
	string? lastName,
	string? pictureUrl,
	string? phoneNumber,
	string? email,
	string? linkedInUrl) : ICommand;

