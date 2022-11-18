using Microsoft.EntityFrameworkCore;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Abstractions.HubSpot;
using Pelican.Application.Abstractions.Messaging;
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
		_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
		_hubSpotClientService = hubSpotClientService ?? throw new ArgumentNullException(nameof(hubSpotClientService));
		_hubSpotAuthorizationService = hubSpotAuthorizationService ?? throw new ArgumentNullException(nameof(hubSpotAuthorizationService));
	}
	public async Task<Result> Handle(UpdateClientCommand command, CancellationToken cancellationToken)
	{
		Client? client = _unitOfWork
			.ClientRepository
			.FindByCondition(d => d.HubSpotId == command.ObjectId.ToString())
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
		UpdateClientCommand command,
		CancellationToken cancellationToken = default)
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

		_unitOfWork
			.ClientRepository
			.Update(client);

		await _unitOfWork.SaveAsync(cancellationToken);

		return Result.Success();
	}
	private async Task<Client> FillOutClientAssociationsAsync(
		Client client,
		CancellationToken cancellationToken = default)
	{
		List<Contact> contacts = new();

		foreach (ClientContact item in client.ClientContacts)
		{
			Contact? matchingContact = await _unitOfWork
				.ContactRepository
				.FirstOrDefaultAsync(
					d => d.HubSpotId == item.HubSpotContactId,
					cancellationToken);

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

		await FillOutClientAssociationsAsync(result.Value, cancellationToken);

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
		Result<string> accessTokenResult = await _hubSpotAuthorizationService.RefreshAccessTokenAsync(
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
						portalId,
						clientHubSpotId,
						cancellationToken);

		if (result.IsFailure)
		{
			return result;
		}

		client.UpdateClientContacts(result.Value.ClientContacts);

		return client;
	}
}
