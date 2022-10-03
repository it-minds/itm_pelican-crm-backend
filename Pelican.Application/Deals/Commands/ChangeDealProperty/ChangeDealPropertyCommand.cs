using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.HubSpot.Commands.NewInstallation;
using Pelican.Domain.Repositories;
using Pelican.Domain.Shared;

namespace Pelican.Application.Deals.Commands.ChangeDealProperty;
public sealed record ChangeDealPropertyCommand(
	long ObjectId,
	string PropertyName,
	string PropertyValue) : ICommand;

internal sealed class ChangeDealPropertyCommandHandler : ICommandHandler<NewInstallationCommand>
{
	readonly IDealRepository _dealRepository;
	public ChangeDealPropertyCommandHandler(IDealRepository dealRepository)
	{
		_dealRepository = dealRepository;
	}
	public async Task<Result> Handle(NewInstallationCommand request, CancellationToken cancellationToken)
	{

		throw new NotImplementedException();
	}
}
