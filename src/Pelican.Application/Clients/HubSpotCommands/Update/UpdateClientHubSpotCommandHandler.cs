using Microsoft.EntityFrameworkCore;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Abstractions.HubSpot;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Domain;
using Pelican.Domain.Entities;
using Pelican.Domain.Shared;

namespace Pelican.Application.Clients.HubSpotCommands.UpdateClient;
internal sealed class UpdateClientHubSpotCommandHandler : ICommandHandler<UpdateClientHubSpotCommand>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IHubSpotObjectService<Client> _hubSpotClientService;
	private readonly IHubSpotAuthorizationService _hubSpotAuthorizationService;
	public UpdateClientHubSpotCommandHandler(
		IUnitOfWork unitOfWork,
		IHubSpotObjectService<Client> hubSpotClientService,
		IHubSpotAuthorizationService hubSpotAuthorizationService)
	{
		_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
		_hubSpotClientService = hubSpotClientService ?? throw new ArgumentNullException(nameof(hubSpotClientService));
		_hubSpotAuthorizationService = hubSpotAuthorizationService ?? throw new ArgumentNullException(nameof(hubSpotAuthorizationService));
	}
	public async Task<Result> Handle(UpdateClientHubSpotCommand command, CancellationToken cancellationToken)
	{
		Client? client = _unitOfWork
			.ClientRepository
			.FindByCondition(d => d.SourceId == command.ObjectId.ToString() && d.Source == Sources.HubSpot)
			.Include(x => x.ClientContacts)
			.Include(x => x.Deals)
			.FirstOrDefault();

		if (client is null)
		{
			return await GetAndCreateClientAsync(
				command.ObjectId,
				command.PortalId,
				cancellationToken);
		}

		return await UpdateExistingClient(
			client,
			command,
			cancellationToken);
	}
	private async Task<Result> UpdateExistingClient(
		Client client,
		UpdateClientHubSpotCommand command,
		CancellationToken cancellationToken = default)
	{
		if ((client.LastUpdatedAt ?? client.CreatedAt) <= command.UpdateTime)
		{
			if (command.PropertyName is "num_associated_contacts" or "num_associated_deals")
			{
				Result result = await UpdateAssociationAsync(
					client,
					command.PortalId,
					command.ObjectId,
					command.PropertyName,
					cancellationToken);

				if (result.IsFailure)
				{
					return result;
				}
			}

			else
			{
				client.UpdateProperty(
					command.PropertyName,
					command.PropertyValue);
			}
		}

		else
		{
			Result<Client> result = await GetClientFromHubSpot(
						command.ObjectId,
						command.PortalId,
						cancellationToken);

			if (result.IsFailure)
			{
				return result;
			}

			client.UpdatePropertiesFromClient(result.Value);
		}

		await _unitOfWork.SaveAsync(cancellationToken);

		return Result.Success();
	}

	private async Task<Result> GetAndCreateClientAsync(
		long objectId,
		long portalId,
		CancellationToken cancellationToken = default)
	{
		Result<Client> result = await GetClientFromHubSpot(
			objectId,
			portalId,
			cancellationToken);

		if (result.IsFailure)
		{
			return result;
		}

		await _unitOfWork
			.ClientRepository
			.CreateAsync(
				result.Value,
				cancellationToken);

		await _unitOfWork.SaveAsync(cancellationToken);

		return Result.Success();
	}

	private async Task<Result<Client>> GetClientFromHubSpot(
		long objectId,
		long portalId,
		CancellationToken cancellationToken = default)
	{
		Result<string> accessTokenResult = await _hubSpotAuthorizationService.RefreshAccessTokenFromSupplierHubSpotIdAsync(
			portalId,
			_unitOfWork,
			cancellationToken);

		if (accessTokenResult.IsFailure)
		{
			return Result.Failure<Client>(accessTokenResult.Error);
		}

		return await _hubSpotClientService.GetByIdAsync(
			accessTokenResult.Value,
			objectId,
			cancellationToken);
	}

	private async Task<Result<Client>> UpdateAssociationAsync(
		Client client,
		long portalId,
		long clientHubSpotId,
			string propertyName,
		CancellationToken cancellationToken = default)
	{
		Result<Client> result = await GetClientFromHubSpot(
			clientHubSpotId,
			portalId,
			cancellationToken);

		if (result.IsFailure)
		{
			return result;
		}

		if (propertyName == "num_associated_contacts")
		{
			client.UpdateClientContacts(result.Value.ClientContacts);
		}
		else
		{
			client.UpdateDeals(result.Value.Deals);
		}

		_unitOfWork.ClientRepository.Attach(client);

		return client;
	}
}
