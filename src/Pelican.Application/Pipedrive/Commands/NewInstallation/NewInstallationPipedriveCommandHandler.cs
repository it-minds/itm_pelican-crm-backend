using Pelican.Application.Abstractions.Messaging;
using Pelican.Domain.Shared;

namespace Pelican.Application.Pipedrive.Commands.NewInstallation;

internal sealed class NewInstallationPipedriveCommandHandler : ICommandHandler<NewInstallationPipedriveCommand>
{
	public Task<Result> Handle(NewInstallationPipedriveCommand request, CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}
}
