using Microsoft.EntityFrameworkCore;
using Pelican.Application.Abstractions.HubSpot;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.Common.Interfaces.Repositories;
using Pelican.Domain.Entities;
using Pelican.Domain.Shared;

namespace Pelican.Application.Deals.Commands.UpdateDeal;

internal sealed class UpdateDealCommandHandler : ICommandHandler<UpdateDealCommand>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IHubSpotObjectService<Deal> _hubSpotDealService;
	private readonly IHubSpotAuthorizationService _hubSpotAuthorizationService;

	public UpdateDealCommandHandler(
		IUnitOfWork unitOfWork,
		IHubSpotObjectService<Deal> hubSpotDealService,
		IHubSpotAuthorizationService hubSpotAuthorizationService)
	{
		_unitOfWork = unitOfWork
			?? throw new ArgumentNullException(nameof(unitOfWork));

		_hubSpotDealService = hubSpotDealService
			?? throw new ArgumentNullException(nameof(hubSpotDealService));

		_hubSpotAuthorizationService = hubSpotAuthorizationService
			?? throw new ArgumentNullException(nameof(hubSpotAuthorizationService));
	}

	public async Task<Result> Handle(UpdateDealCommand command, CancellationToken cancellationToken = default)
	{
		Deal? deal = await _unitOfWork
			.DealRepository
			.FirstOrDefaultAsync(
				d => d.HubSpotId == command.ObjectId.ToString(),
				cancellationToken);

		if (deal is null)
		{
			return await GetDealFromHubSpotAsync(
				command.UserId,
				command.ObjectId,
				cancellationToken);
		}

		deal.UpdateProperty(
			command.PropertyName,
			command.PropertyValue);

		_unitOfWork
			.DealRepository
			.Update(deal);

		await _unitOfWork.SaveAsync(cancellationToken);

		return Result.Success();
	}

	private async Task<Result> GetDealFromHubSpotAsync(
		string userId,
		long objectId,
		CancellationToken cancellationToken)
	{
		Result<string> accessTokenResult = await GetAccessTokenAsync(userId, cancellationToken);

		if (accessTokenResult.IsFailure)
		{
			return Result.Failure<Deal>(accessTokenResult.Error);
		}

		Result<Deal> result = await _hubSpotDealService
			.GetByIdAsync(
				accessTokenResult.Value,
				objectId,
				cancellationToken);

		if (result.IsFailure)
		{
			return Result.Failure<Deal>(result.Error);
		}

		Deal deal = result.Value;
		deal = await AddAccountManagerAsync(deal, cancellationToken);
		deal = await AddClientAsync(deal, cancellationToken);
		deal = await AddContactsAsync(deal, cancellationToken);

		await _unitOfWork
			.DealRepository
			.CreateAsync(deal, cancellationToken);

		await _unitOfWork.SaveAsync(cancellationToken);

		return Result.Success();
	}

	private async Task<Result<string>> GetAccessTokenAsync(
		string userId,
		CancellationToken cancellationToken = default)
	{
		Supplier? supplier = await _unitOfWork
				.SupplierRepository
				.FirstOrDefaultAsync(
					supplier => supplier.HubSpotId.ToString() == userId,
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

	private async Task<Deal> AddContactsAsync(
		Deal deal,
		CancellationToken cancellationToken)
	{
		List<Contact>? contacts = await _unitOfWork
			.ContactRepository
			.FindByCondition(c => c.HubSpotId == deal.HubSpotId)
			.ToListAsync(cancellationToken);

		return deal.AttandContacts(contacts);

	}

	private async Task<Deal> AddAccountManagerAsync(
		Deal deal,
		CancellationToken cancellationToken = default)
	{
		AccountManager? accountManager = await _unitOfWork
			.AccountManagerRepository
			.FirstOrDefaultAsync(a => a.HubSpotId == deal.HubSpotOwnerId, cancellationToken);

		return deal.AttachAccountmManager(accountManager);
	}

	private async Task<Deal> AddClientAsync(
		Deal deal,
		CancellationToken cancellationToken = default)
	{
		Client? client = await _unitOfWork
			.ClientRepository
			.FirstOrDefaultAsync(c => c.HubSpotId == deal.Client!.HubSpotId, cancellationToken);

		return deal.AttachClient(client);
	}
}
