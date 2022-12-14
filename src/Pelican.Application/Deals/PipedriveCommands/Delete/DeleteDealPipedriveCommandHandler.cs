using Pelican.Application.Abstractions.Messaging;
using Pelican.Domain.Shared;

namespace Pelican.Application.Deals.PipedriveCommands.Delete;

internal sealed class DeleteDealPipedriveCommandHandler : ICommandHandler<DeleteDealPipedriveCommand>
{
	public Task<Result> Handle(DeleteDealPipedriveCommand request, CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}
}
