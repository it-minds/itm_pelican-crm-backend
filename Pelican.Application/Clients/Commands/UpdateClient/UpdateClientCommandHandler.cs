using Pelican.Application.Abstractions.HubSpot;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.Common.Interfaces.Repositories;
using Pelican.Domain.Entities;
using Pelican.Domain.Shared;

namespace Pelican.Application.Clients.Commands.UpdateClient;
internal sealed class UpdateClientCommandHandler : ICommandHandler<UpdateClientCommand>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IHubSpotObjectService<Client> _hubSpotClientService;
	private readonly IHubSpotAuthorizationService _hubSpotAuthorizationService;
	public UpdateClientCommandHandler(
		IUnitOfWork unitOfWork,
		IHubSpotObjectService<Client> hubSpotClientService,
		IHubSpotAuthorizationService hubSpotAuthorizationService)
	{
		_unitOfWork = unitOfWork;
		_hubSpotClientService = hubSpotClientService;
		_hubSpotAuthorizationService = hubSpotAuthorizationService;
	}
	public async Task<Result> Handle(UpdateClientCommand command, CancellationToken cancellationToken)
	{
		Client? client = _unitOfWork
			.ClientRepository
			.FindByCondition(d => d.HubSpotId == command.ObjectId.ToString())
			.FirstOrDefault();

		if (client is null)
		{
			return await GetClientFromHubSpot(
				command.PortalId,
				command.ObjectId,
				cancellationToken);
		}

		switch (command.PropertyName)
		{
			case "name":
				client.Name = command.PropertyValue;
				break;
			case "city":
				client.OfficeLocation = command.PropertyValue;
				break;
			case "industry":
				client.Segment = command.PropertyValue;
				break;
			case "website":
				client.Website = command.PropertyValue;
				break;
			case "num_associated_contacts":
				client.ClientContacts = UpdateClientContacts(client);
				break;
			default:
				break;
		}

		_unitOfWork
			.ClientRepository
			.Update(client);

		await _unitOfWork.SaveAsync(cancellationToken);

		return Result.Success();
	}

	private async Task<Result> GetClientFromHubSpot(long portalId, long objectId, CancellationToken cancellationToken)
	{
		Supplier? supplier = _unitOfWork
				.SupplierRepository
				.FindByCondition(supplier => supplier.HubSpotId == portalId)
				.FirstOrDefault();

		if (supplier is null || supplier.RefreshToken is null or "")
		{
			return Result.Failure<Client>(Error.NullValue);
		}

		Result<string> accessTokenResult = await _hubSpotAuthorizationService
			.RefreshAccessTokenAsync(supplier.RefreshToken, cancellationToken);

		if (accessTokenResult.IsFailure)
		{
			return Result.Failure<Client>(accessTokenResult.Error);
		}

		Result<Client> result = await _hubSpotClientService.GetByIdAsync(accessTokenResult.Value, objectId, cancellationToken);

		if (result.IsFailure)
		{
			return Result.Failure<Client>(result.Error);
		}

		await _unitOfWork
			.ClientRepository
			.CreateAsync(result.Value, cancellationToken);
		await _unitOfWork.SaveAsync(cancellationToken);
		return Result.Success();
	}

	private ICollection<ClientContact> UpdateClientContacts(Client client)
	{
		ICollection<ClientContact> localClientContacts = _unitOfWork
					.ClientContactRepository
					.FindByCondition(localClientContacts
						=> localClientContacts.ClientId == client.Id)
					.ToList();
		if (client.ClientContacts is not null)
		{
			foreach (var item in localClientContacts)
			{
				if (!client.ClientContacts.Contains(item))
				{
					item.IsActive = false;
				}
			}
			foreach (var item in client.ClientContacts)
			{
				if (!localClientContacts.Contains(item))
				{
					localClientContacts.Add(item);
				}
			}
		}
		return client.ClientContacts;
	}
}
