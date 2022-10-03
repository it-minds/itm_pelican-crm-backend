using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.HubSpot.Commands.NewInstallation;
using Pelican.Domain.Shared;

namespace Pelican.Application.Deals.Commands.ChangeDealProperty;
public sealed record ChangeDealPropertyCommand(
	long ObjectId,
	string PropertyName,
	string PropertyValue) : ICommand;


internal sealed class ChangeDealPropertyCommandHandler : ICommandHandler<NewInstallationCommand>
{
	public Task<Result> Handle(NewInstallationCommand request, CancellationToken cancellationToken)
	{

		throw new NotImplementedException();
	}
}
