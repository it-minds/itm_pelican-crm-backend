using Pelican.Application.Abstractions.Messaging;

namespace Pelican.Application.AccountManagers.PipedriveCommands.Delete;

public sealed record DeleteAccountManagerPipedriveCommand(int accountManagerId) : ICommand;
