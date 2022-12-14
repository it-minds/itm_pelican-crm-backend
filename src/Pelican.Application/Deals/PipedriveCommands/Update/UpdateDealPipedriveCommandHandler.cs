using Pelican.Application.Abstractions.Messaging;
using Pelican.Domain.Shared;

namespace Pelican.Application.Deals.PipedriveCommands.Update;

internal sealed class UpdateDealPipedriveCommandHandler : ICommandHandler<UpdateDealPipedriveCommand>
{
	public Task<Result> Handle(UpdateDealPipedriveCommand request, CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}
}
