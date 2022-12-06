using Pelican.Application.Abstractions.Messaging;
using Pelican.Domain.Shared;

namespace Pelican.Application.Contacts.PipedriveCommands.Update;
internal sealed class UpdateContactPipedriveCommandHandler : ICommandHandler<UpdateContactPipedriveCommand>
{
	public Task<Result> Handle(UpdateContactPipedriveCommand request, CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}
}
