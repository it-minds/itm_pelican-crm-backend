using Pelican.Application.Abstractions.Messaging;
using Pelican.Domain.Shared;

namespace Pelican.Application.Contacts.PipedriveCommands.Delete;

internal sealed class DeleteContactPipedriveCommandHandler : ICommandHandler<DeleteContactPipedriveCommand>
{
	public Task<Result> Handle(DeleteContactPipedriveCommand request, CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}
}
