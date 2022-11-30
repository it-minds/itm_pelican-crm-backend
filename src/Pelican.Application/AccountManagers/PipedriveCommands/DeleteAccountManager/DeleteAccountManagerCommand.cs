using Pelican.Application.Abstractions.Messaging;

namespace Pelican.Application.AccountManagers.PipedriveCommands.DeleteAccountManager;

public sealed record DeleteAccountManagerPipedriveCommand(int accountManagerId) : ICommand;
