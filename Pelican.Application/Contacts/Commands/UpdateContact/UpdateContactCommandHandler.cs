using Pelican.Application.Abstractions.HubSpot;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.Common.Interfaces.Repositories;
using Pelican.Domain.Entities;
using Pelican.Domain.Shared;

namespace Pelican.Application.Contacts.Commands.UpdateContact;

internal sealed class UpdateContactCommandHandler : ICommandHandler<UpdateContactCommand>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IHubSpotObjectService<Contact> _hubSpotContactService;
	private readonly IHubSpotAuthorizationService _hubSpotAuthorizationService;
	public UpdateContactCommandHandler(
		IUnitOfWork unitOfWork,
		IHubSpotObjectService<Contact> hubSpotContactService,
		IHubSpotAuthorizationService hubSpotAuthorizationService)
	{
		_unitOfWork = unitOfWork
			?? throw new ArgumentNullException(nameof(unitOfWork));

		_hubSpotContactService = hubSpotContactService
			?? throw new ArgumentNullException(nameof(hubSpotContactService));

		_hubSpotAuthorizationService = hubSpotAuthorizationService
			?? throw new ArgumentNullException(nameof(hubSpotAuthorizationService));
	}

	public async Task<Result> Handle(
		UpdateContactCommand command,
		CancellationToken cancellationToken = default)
	{
		Contact? contact = await _unitOfWork
			.ContactRepository
			.FirstOrDefaultAsync(
				contact => contact.HubSpotId == command.ObjectId.ToString(),
				cancellationToken);

		if (contact is null)
		{
			return await GetAndCreateContactAsync(
				command.SupplierHubSpotId,
				command.ObjectId,
				cancellationToken);
		}

		return await UpdateExistingContact(
			contact,
			command,
			cancellationToken);
	}

	private async Task<Result> UpdateExistingContact(
		Contact contact,
		UpdateContactCommand command,
		CancellationToken cancellationToken = default)
	{
		if (command.PropertyName == "num_associated_deals")
		{
			Result<Contact> result = await UpdateDealContactsAsync(
				contact,
				command.SupplierHubSpotId,
				command.ObjectId,
				cancellationToken);

			if (result.IsFailure)
			{
				return result;
			}
		}
		else
		{
			contact.UpdateProperty(
				command.PropertyName,
				command.PropertyValue);
		}

		_unitOfWork
			.ContactRepository
			.Update(contact);

		await _unitOfWork.SaveAsync(cancellationToken);

		return Result.Success();
	}

	private async Task<Result<Contact>> UpdateDealContactsAsync(
		Contact contact,
		long supplierHubSpotId,
		long contactHubSpotId,
		CancellationToken cancellationToken = default)
	{
		Result<Contact> result = await GetContactFromHubSpot(
						supplierHubSpotId,
						contactHubSpotId,
						cancellationToken);

		if (result.IsFailure)
		{
			return result;
		}

		contact.UpdateDealContacts(result.Value.DealContacts);

		return contact;
	}

	private async Task<Result> GetAndCreateContactAsync(
		long supplierHubSpotId,
		long objectId,
		CancellationToken cancellationToken = default)
	{
		Result<Contact> result = await GetContactFromHubSpot(
						supplierHubSpotId,
						objectId,
						cancellationToken);

		if (result.IsFailure)
		{
			return result;
		}

		await FillOutContactAssociationsAsync(result.Value, cancellationToken);

		await _unitOfWork
			.ContactRepository
			.CreateAsync(
				result.Value,
				cancellationToken);

		await _unitOfWork.SaveAsync(cancellationToken);

		return Result.Success();
	}

	private async Task<Contact> FillOutContactAssociationsAsync(
		Contact contact,
		CancellationToken cancellationToken = default)
	{
		List<Deal> deals = new();

		foreach (DealContact dealContact in contact.DealContacts)
		{
			Deal? deal = await _unitOfWork
				.DealRepository
				.FirstOrDefaultAsync(
					d => d.HubSpotId == dealContact.Deal.HubSpotId,
					cancellationToken);

			if (deal is not null)
			{
				deals.Add(deal);
			}
		}

		List<Client> clients = new();

		foreach (ClientContact clientContact in contact.ClientContacts)
		{
			Client? client = await _unitOfWork
				.ClientRepository
				.FirstOrDefaultAsync(
					c => c.HubSpotId == clientContact.Client.HubSpotId,
					cancellationToken);

			if (client is not null)
			{
				clients.Add(client);
			}
		}

		contact.FillOutAssociations(clients, deals);

		return contact;
	}

	private async Task<Result<Contact>> GetContactFromHubSpot(
		long supplierHubSpotId,
		long objectId,
		CancellationToken cancellationToken = default)
	{
		Result<string> accessTokenResult = await _hubSpotAuthorizationService.RefreshAccessTokenAsync(
			supplierHubSpotId,
			_unitOfWork,
			cancellationToken);

		if (accessTokenResult.IsFailure)
		{
			return Result.Failure<Contact>(accessTokenResult.Error);
		}

		return await _hubSpotContactService.GetByIdAsync(
			accessTokenResult.Value,
			objectId,
			cancellationToken);
	}
}
