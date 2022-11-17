using Microsoft.EntityFrameworkCore;
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
	public UpdateContactCommandHandler(
		IUnitOfWork unitOfWork,
		IHubSpotObjectService<Contact> hubSpotContactService)
	{
		_unitOfWork = unitOfWork;
		_hubSpotContactService = hubSpotContactService;
	}
	public async Task<Result> Handle(UpdateContactCommand command, CancellationToken cancellationToken)
	{
		Contact? contact = _unitOfWork
			.ContactRepository
			.FindByCondition(contact => contact.Id.ToString() == command.ObjectId.ToString())
			.FirstOrDefault();

		if (contact is null)
		{
			return await GetContactFromHubSpot(
				command.PortalId,
				command.ObjectId,
				cancellationToken);
		}

		switch (command.PropertyName)
		{
			case "firstname":
				contact.Firstname = command.PropertyValue;
				break;
			case "lastname":
				contact.Lastname = command.PropertyValue;
				break;
			case "email":
				contact.Email = command.PropertyValue;
				break;
			case "phone":
			case "mobilephone":
				contact.PhoneNumber = command.PropertyValue;
				break;
			case "company":
				var oldClientContactRelation = contact
					.ClientContacts
					.FirstOrDefault(clientContact
						=> clientContact.Id.ToString() == command.ObjectId.ToString()
						|| clientContact.IsActive is true);
				if (oldClientContactRelation is not null)
				{
					oldClientContactRelation.IsActive = false;
				}
				break;
			default:
				break;
		}

		_unitOfWork
			.ContactRepository
			.Update(contact);

		await _unitOfWork.SaveAsync(cancellationToken);

		return Result.Success();
	}

	private async Task<Result> GetContactFromHubSpot(string userId, long objectId, CancellationToken cancellationToken)
	{
		Supplier? supplier = await _unitOfWork
				.SupplierRepository
				.FindByCondition(supplier => supplier.HubSpotId.ToString() == userId)
				.FirstOrDefaultAsync();

		if (supplier is null)
		{
			return Result.Failure<Contact>(Error.NullValue);
		}

		Result<Contact> result = await _hubSpotContactService.GetByIdAsync(supplier.RefreshToken, objectId, cancellationToken);

		if (result.IsFailure)
		{
			return Result.Failure<Contact>(result.Error);
		}

		await _unitOfWork
			.ContactRepository
			.CreateAsync(result.Value, cancellationToken);

		return Result.Success();
	}
}
