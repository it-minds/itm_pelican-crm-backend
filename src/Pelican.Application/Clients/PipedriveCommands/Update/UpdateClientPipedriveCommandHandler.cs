using Pelican.Application.Abstractions.Messaging;
using Pelican.Domain.Shared;

namespace Pelican.Application.Clients.PipedriveCommands.Update;
internal sealed class UpdateClientPipedriveCommandHandler : ICommandHandler<UpdateClientPipedriveCommand>
{
	public Task<Result> Handle(UpdateClientPipedriveCommand request, CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}
}
