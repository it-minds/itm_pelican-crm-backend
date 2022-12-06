using Pelican.Application.Abstractions.Messaging;
using Pelican.Domain.Shared;

namespace Pelican.Application.Clients.PipedriveCommands.Delete;

internal sealed class DeleteClientPipedriveCommandHandler : ICommandHandler<DeleteClientPipedriveCommand>
{
	public Task<Result> Handle(DeleteClientPipedriveCommand request, CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}
}
