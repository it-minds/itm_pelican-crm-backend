using Pelican.Application.Abstractions.Messaging;

namespace Pelican.Application.Clients.PipedriveCommands.Update;
public sealed record UpdateClientPipedriveCommand(
	int supplierPipedriveId,
	int clientId,
	int clientPipedriveOwnerId,
	string clientName,
	string? officeLocation,
	string? website) : ICommand;

