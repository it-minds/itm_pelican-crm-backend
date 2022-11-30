using Pelican.Application.Abstractions.Messaging;
using Pelican.Domain.Shared;

namespace Pelican.Application.AccountManagers.PipedriveCommands.UpdateAccountManager;
internal sealed class UpdateAccountManagerPipedriveCommandHandler : ICommandHandler<UpdateAccountManagerPipedriveCommand>
{
	public Task<Result> Handle(UpdateAccountManagerPipedriveCommand request, CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}
}
