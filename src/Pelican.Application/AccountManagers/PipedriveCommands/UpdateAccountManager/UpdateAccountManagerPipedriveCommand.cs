using Pelican.Application.Abstractions.Messaging;

namespace Pelican.Application.AccountManagers.PipedriveCommands.UpdateAccountManager;
public sealed record UpdateClientPipedriveCommand(
	int SupplierPipedriveId,
	int AccountManagerId,
	int AccountManagerPipedriveId,
	string? name,
	string) : ICommand;
