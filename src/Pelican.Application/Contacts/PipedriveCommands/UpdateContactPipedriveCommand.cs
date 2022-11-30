using Pelican.Application.Abstractions.Messaging;

namespace Pelican.Application.Contacts.PipedriveCommands;
public sealed record UpdateContactPipedriveCommand(
	int SupplierPipedriveId,
	int DealId,
	int DealPipedriveOwnerId,
	int DealStatusId,
	string? DealDescription,
	string? DealName,
	string? LastContactDate,
	string? StartDate,
	string? EndDate) : ICommand;

