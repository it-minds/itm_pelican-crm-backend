using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Pelican.Application.Test")]
namespace Pelican.Application.Pipedrive.Commands.NewInstallation;

internal sealed class NewInstallationCommandHandler : ICommandHandler<NewInstallationPipedriveCommand>
{
	public Task<Result> Handle(NewInstallationPipedriveCommand request, CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}
}
