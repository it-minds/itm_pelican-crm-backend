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
			return await GetContactFromHubSpot(
				command.SupplierHubSpotId,
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

	private async Task<Result> GetContactFromHubSpot(
		long supplierHubSpotId,
		long objectId,
		CancellationToken cancellationToken = default)
	{
		Supplier? supplier = await _unitOfWork
			.SupplierRepository
			.FirstOrDefaultAsync(
				supplier => supplier.HubSpotId == supplierHubSpotId,
				cancellationToken);

		if (supplier is null)
		{
			return Result.Failure<Contact>(Error.NullValue);
		}

		Result<Contact> result = await _hubSpotContactService.GetByIdAsync(
			supplier.RefreshToken,
			objectId,
			cancellationToken);

		if (result.IsFailure)
		{
			return Result.Failure<Contact>(result.Error);
		}

		await _unitOfWork
			.ContactRepository
			.CreateAsync(
				result.Value,
				cancellationToken);

		return Result.Success();
	}
}
