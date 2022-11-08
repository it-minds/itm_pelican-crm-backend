using Microsoft.EntityFrameworkCore;
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
			.Include(x => x.ClientContacts)
			.ThenInclude(x => x.Contact)
			.FirstOrDefault();

		if (client is null)
		{
			Result<string> accessTokenResult = GetAccessToken(command.PortalId, cancellationToken).Result;
			if (accessTokenResult.IsFailure)
			{
				return Result.Failure(accessTokenResult.Error);
			}
			return await GetClientFromHubSpot(
				command.PortalId, accessTokenResult.Value, cancellationToken);
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
				return await UpdateClientContacts(client, command.ObjectId, command.PortalId, cancellationToken);
			default:
				throw new ArgumentException($"{command.PropertyName} is not a valid property on Client");
		}

		_unitOfWork
			.ClientRepository
			.Update(client);

		await _unitOfWork.SaveAsync(cancellationToken);

		return Result.Success();
	}
	private async Task<Result<string>> GetAccessToken(long portalId, CancellationToken cancellationToken)
	{
		Supplier? supplier = _unitOfWork
		.SupplierRepository
				.FindByCondition(supplier => supplier.HubSpotId == portalId)
				.FirstOrDefault();

		if (supplier is null || string.IsNullOrWhiteSpace(supplier.RefreshToken))
		{
			return Result.Failure<string>(Error.NullValue);
		}

		Result<string> accessTokenResult = await _hubSpotAuthorizationService
			.RefreshAccessTokenAsync(supplier.RefreshToken, cancellationToken);
		if (accessTokenResult.IsFailure)
		{
			return Result.Failure<string>(accessTokenResult.Error);
		}
		return accessTokenResult;
	}

	private async Task<Result> GetClientFromHubSpot(long objectId, string accessToken, CancellationToken cancellationToken)
	{
		Result<Client> result = await _hubSpotClientService.GetByIdAsync(accessToken, objectId, cancellationToken);
		if (result.IsFailure)
			return result;
		foreach (var item in result.Value.ClientContacts)
		{
			var contact = _unitOfWork.ContactRepository.FindByCondition(x => x.HubSpotId == item.HubSpotContactId).FirstOrDefault();
			if (contact is not null)
			{
				item.Contact = contact;
				item.ContactId = contact.Id;
			}
		}
		result.Value.ClientContacts = result.Value.ClientContacts.Where(x => x.Contact is not null).ToList();

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

	private async Task<Result> UpdateClientContacts(Client localClient, long portalId, long objectId, CancellationToken cancellationToken)
	{
		Result<string> accessTokenResult = GetAccessToken(portalId, cancellationToken).Result;
		if (accessTokenResult.IsFailure)
		{
			return Result.Failure(accessTokenResult.Error);
		}
		Result<Client> result = await _hubSpotClientService.GetByIdAsync(accessTokenResult.Value, objectId, cancellationToken);

		if (result.IsFailure)
		{
			return Result.Failure<Client>(result.Error);
		}

		foreach (var item in localClient.ClientContacts)
		{
			if (!result.Value.ClientContacts.Any(c => c.HubSpotClientId == item.HubSpotClientId && c.HubSpotContactId == item.HubSpotContactId))
			{
				item.IsActive = false;
			}
		}
		foreach (var item in result.Value.ClientContacts)
		{
			if (!localClient.ClientContacts.Any(c => c.HubSpotClientId == item.HubSpotClientId && c.HubSpotContactId == item.HubSpotContactId))
			{
				var contact = _unitOfWork.ContactRepository.FindByCondition(x => x.HubSpotId == item.HubSpotContactId).FirstOrDefault();
				if (contact is not null)
				{
					item.Contact = contact;
					item.ContactId = contact.Id;
				}
				localClient.ClientContacts.Add(item);
			}
		}
		localClient.ClientContacts = localClient.ClientContacts.Where(x => x.Contact is not null).ToList();
		_unitOfWork.ClientRepository.Update(localClient);
		await _unitOfWork.SaveAsync(cancellationToken);
		return Result.Success();
	}
}
