using System.Runtime.CompilerServices;
using Pelican.Application.Abstractions.HubSpot;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.Common.Interfaces.Repositories;
using Pelican.Domain.Shared;

[assembly: InternalsVisibleTo("Pelican.Application.Test")]
namespace Pelican.Application.HubSpot.Commands.NewInstallation;

internal sealed class NewInstallationCommandHandler : ICommandHandler<NewInstallationCommand>
{
	private readonly IHubSpotAuthorizationService _hubSpotService;
	private readonly IUnitOfWork _unitOfWork;

	public NewInstallationCommandHandler(
		IHubSpotAuthorizationService hubSpotService,
		IUnitOfWork unitOfWork)
	{
		_hubSpotService = hubSpotService;
		_unitOfWork = unitOfWork;
	}

	public async Task<Result> Handle(
		NewInstallationCommand command,
		CancellationToken cancellationToken)
	{
		Result<Tuple<string, string>> tokensResult = await _hubSpotService.AuthorizeUserAsync(command.Code, cancellationToken);

		if (tokensResult.IsFailure)
		{
			return Result.Failure(tokensResult.Error);
		}


		/// Needs furhter development to fetch data from new installation
		/// Either by raising domain event or just calling functions from here


		return tokensResult;
	}
}
