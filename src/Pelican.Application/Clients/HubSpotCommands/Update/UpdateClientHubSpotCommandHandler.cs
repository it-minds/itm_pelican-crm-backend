﻿using Microsoft.EntityFrameworkCore;
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
			.Include(d => d.ClientContacts)
				.ThenInclude(cc => cc.Contact)
			.Include(d => d.Deals)
			.FirstOrDefault();

		if (client is null)
		{
			return await GetAndCreateClientAsync(
				command.PortalId,
				command.ObjectId,
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
		if (client.SourceUpdateTimestamp <= command.UpdateTime)
		{
			if (command.PropertyName == "num_associated_contacts")
			{
				Result<Client> result = await UpdateClientContactsAsync(
					client,
					command.PortalId,
					command.ObjectId,
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
			client.SourceUpdateTimestamp = command.UpdateTime;
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
			client.Website = result.Value.Website;
			client.Name = result.Value.Name;
			client.OfficeLocation = result.Value.OfficeLocation;
			client.UpdateClientContacts(result.Value.ClientContacts);
		}
		_unitOfWork
				.ClientRepository
				.Update(client);

		await _unitOfWork.SaveAsync(cancellationToken);
		return Result.Success();
	}
	private Client FillOutClientAssociations(
		Client client)
	{
		List<Contact> contacts = new();

		foreach (ClientContact item in client.ClientContacts)
		{
			Contact? matchingContact = _unitOfWork
				.ContactRepository
				.FindByCondition(
					d => d.SourceId == item.SourceContactId && d.Source == Sources.HubSpot)
				.FirstOrDefault();

			if (matchingContact is not null)
			{
				contacts.Add(matchingContact);
			}
		}

		client.FillOutClientContacts(contacts);

		return client;
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

		FillOutClientAssociations(result.Value);

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
	private async Task<Result<Client>> UpdateClientContactsAsync(
		Client client,
		long portalId,
		long clientHubSpotId,
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
		client.UpdateClientContacts(result.Value.ClientContacts);

		var newClientContacts = client.ClientContacts.Where(cc => cc.Contact is null).ToList();

		FillOutClientAssociations(client);

		_unitOfWork.ClientContactRepository.AttachAsAdded(newClientContacts);
		return client;
	}
}