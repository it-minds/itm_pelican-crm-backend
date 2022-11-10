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
			Result<Contact> result = await UpdateDealContacts(
				contact,
				command.SupplierHubSpotId,
				command.ObjectId);

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

	private async Task<Result<Contact>> UpdateDealContacts(
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

		return (Result<Contact>)contact;
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

		Contact contact = FillOutContactAssociations(result.Value);

		await _unitOfWork
			.ContactRepository
			.CreateAsync(
				result.Value,
				cancellationToken);

		await _unitOfWork.SaveAsync(cancellationToken);

		return Result.Success();
	}

	private Contact FillOutContactAssociations(Contact contact)
	{
		IEnumerable<Deal> deals = contact
		.DealContacts
		.Select(async dc => await _unitOfWork
		  	.DealRepository
			.FirstOrDefaultAsync(d => d.HubSpotId == dc.Deal.HubSpotId)
		)
		.Select(dTask => dTask.Result)
		.Where(d => d is not null)!;

		IEnumerable<Client> clients = contact
		.ClientContacts
		.Select(async cc => await _unitOfWork
		  	.ClientRepository
			.FirstOrDefaultAsync(c => c.HubSpotId == cc.Client.HubSpotId)
		)
		.Select(cTask => cTask.Result)
		.Where(c => c is not null)!;

		contact.FillOutAssociations(clients, deals);

		return contact;
	}

	private async Task<Result<Contact>> GetContactFromHubSpot(
		long supplierHubSpotId,
		long objectId,
		CancellationToken cancellationToken = default)
	{
		Result<string> accessTokenResult = await GetAccessTokenAsync(
			supplierHubSpotId,
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

	private async Task<Result<string>> GetAccessTokenAsync(
		long supplierHubSpotId,
		CancellationToken cancellationToken = default)
	{
		Supplier? supplier = await _unitOfWork
				.SupplierRepository
				.FirstOrDefaultAsync(
					supplier => supplier.HubSpotId == supplierHubSpotId,
					cancellationToken);

		if (supplier is null
			|| string.IsNullOrWhiteSpace(supplier.RefreshToken))
		{
			return Result.Failure<string>(Error.NullValue);
		}

		Result<string> accessTokenResult = await _hubSpotAuthorizationService
			.RefreshAccessTokenAsync(
				supplier.RefreshToken,
				cancellationToken);

		return accessTokenResult;
	}
}
