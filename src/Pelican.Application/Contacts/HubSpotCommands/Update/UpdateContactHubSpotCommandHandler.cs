using Microsoft.EntityFrameworkCore;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Abstractions.HubSpot;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Domain;
using Pelican.Domain.Entities;
using Pelican.Domain.Shared;

namespace Pelican.Application.Contacts.HubSpotCommands.Update;

internal sealed class UpdateContactHubSpotCommandHandler : ICommandHandler<UpdateContactHubSpotCommand>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IHubSpotObjectService<Contact> _hubSpotContactService;
	private readonly IHubSpotAuthorizationService _hubSpotAuthorizationService;

	public UpdateContactHubSpotCommandHandler(
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
		UpdateContactHubSpotCommand command,
		CancellationToken cancellationToken = default)
	{
		Contact? contact = _unitOfWork
			.ContactRepository
			.FindByCondition(contact => contact.SourceId == command.ObjectId.ToString() && contact.Source == Sources.HubSpot)
			.Include(c => c.ClientContacts)
			.Include(c => c.DealContacts)
			.FirstOrDefault();

		if (contact is null)
		{
			return await GetAndCreateContactAsync(
				command.SupplierHubSpotId,
				command.ObjectId,
				cancellationToken);
		}

		if ((contact.LastUpdatedAt ?? contact.CreatedAt) <= command.UpdateTime)
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
		}
		else
		{
			Result<Contact> result = await GetContactFromHubSpotAsync(
				command.SupplierHubSpotId,
				command.ObjectId,
				cancellationToken);

			if (result.IsFailure)
			{
				return result;
			}

			contact.UpdatePropertiesFromContact(result.Value);
		}

		_unitOfWork
			.ContactRepository
			.Attach(contact);

		await _unitOfWork.SaveAsync(cancellationToken);

		return Result.Success();
	}

	private async Task<Result<Contact>> UpdateDealContactsAsync(
		Contact contact,
		long supplierHubSpotId,
		long contactHubSpotId,
		CancellationToken cancellationToken = default)
	{
		Result<Contact> result = await GetContactFromHubSpotAsync(
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
		Result<Contact> result = await GetContactFromHubSpotAsync(
			supplierHubSpotId,
			objectId,
			cancellationToken);

		if (result.IsFailure)
		{
			return result;
		}

		_unitOfWork
			.ContactRepository
			.Attach(result.Value);

		await _unitOfWork.SaveAsync(cancellationToken);

		return Result.Success();
	}

	private async Task<Result<Contact>> GetContactFromHubSpotAsync(
		long supplierHubSpotId,
		long objectId,
		CancellationToken cancellationToken = default)
	{
		Result<string> accessTokenResult = await _hubSpotAuthorizationService
			.RefreshAccessTokenFromSupplierHubSpotIdAsync(
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
}
