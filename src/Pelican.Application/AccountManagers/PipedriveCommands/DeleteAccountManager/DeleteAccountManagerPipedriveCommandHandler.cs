﻿using Pelican.Application.Abstractions.Messaging;
using Pelican.Domain.Shared;

namespace Pelican.Application.AccountManagers.PipedriveCommands.DeleteAccountManager;

internal sealed class DeleteAccountManagerPipedriveCommandHandler : ICommandHandler<DeleteAccountManagerPipedriveCommand>
{
	public Task<Result> Handle(DeleteAccountManagerPipedriveCommand request, CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}
}

