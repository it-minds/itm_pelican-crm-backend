using Pelican.Application.Abstractions.Messaging;

namespace Pelican.Application.Clients.PipedriveCommands.UpdateClient;
public sealed record UpdateClientPipedriveCommand(
	int supplierPipedriveId,
	int clientId,
	int clientPipedriveOwnerId,
	string clientName,
	string? pictureUrl,
	string? officeLocation,
	string? website) : ICommand;

