using Pelican.Application.Abstractions.HubSpot;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.Common.Interfaces.Repositories;
using Pelican.Domain.Entities;
using Pelican.Domain.Shared;

namespace Pelican.Application.Contacts.Commands.UpdateContact;

internal sealed class UpdateContactCommandHandler : ICommandHandler<UpdateContactCommand>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IHubSpotContactService _hubSpotContactService;
	public UpdateContactCommandHandler(
		IUnitOfWork unitOfWork,
		IHubSpotContactService hubSpotContactService)
	{
		_unitOfWork = unitOfWork;
		_hubSpotContactService = hubSpotContactService;
	}
	public async Task<Result> Handle(UpdateContactCommand command, CancellationToken cancellationToken)
	{
		Contact? contact = _unitOfWork
			.ContactRepository
			.FindByCondition(d => d.Id.ToString() == command.ObjectId.ToString())
			.FirstOrDefault();

		if (contact is null)
		{
			return await GetContactFromHubSpot(
				command.UserId,
				command.ObjectId,
				cancellationToken);
		}

		switch (command.PropertyName)
		{

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
		Contact? contact = _unitOfWork
				.ContactRepository
				.FindByCondition(contact => contact.HubSpotId.ToString() == userId)
				.FirstOrDefault();

		if (contact is null)
		{
			return Result.Failure<Contact>(Error.NullValue);
		}

		string token = contact.RefreshToken;

		Result<Contact> result = await _hubSpotContactService.GetContactByIdAsync(token, objectId, cancellationToken);

		if (result.IsFailure)
		{
			return Result.Failure<Contact>(result.Error);
		}

		_unitOfWork
			.ContactRepository
			.Create(result.Value);

		return Result.Success();
	}
}
