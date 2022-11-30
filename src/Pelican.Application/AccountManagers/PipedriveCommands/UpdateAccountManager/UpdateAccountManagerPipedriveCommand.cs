namespace Pelican.Application.AccountManagers.PipedriveCommands.UpdateAccountManager;
public sealed record UpdateAccountManagerPipedriveCommand(
	int SupplierPipedriveId,
	int AccountManagerId,
	int AccountManagerPipedriveId,
	string firstName,
	string? lastName,
	string? pictureUrl,
	string? phoneNumber,
	string? email,
	string? linkedin) : ICommand;
