using Microsoft.EntityFrameworkCore;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Abstractions.HubSpot;
using Pelican.Application.Abstractions.Messaging;
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
		Deal? deal = _unitOfWork
			.DealRepository
			.FindByCondition(d => d.HubSpotId == command.ObjectId.ToString())
			.Include(d => d.AccountManagerDeals)
			.Include(d => d.Client)
			.Include(d => d.DealContacts)
			.FirstOrDefault();

		if (deal is null)
		{
			return await GetDealFromHubSpotAsync(
				command.SupplierHubSpotId,
				command.ObjectId,
				cancellationToken);
		}

		if (command.PropertyName == "hs_all_owner_ids")
		{
			await UpdateAccountManagerDeal(deal, command.PropertyValue);
		}
		else
		{
			deal.UpdateProperty(
				command.PropertyName,
				command.PropertyValue);
		}

		_unitOfWork
			.DealRepository
			.Update(deal);

		await _unitOfWork.SaveAsync(cancellationToken);

		return Result.Success();
	}

	private async Task UpdateAccountManagerDeal(Deal deal, string acccuntManagerHubSpotId)
	{
		AccountManager? accountManager = await _unitOfWork.
			AccountManagerRepository
			.FirstOrDefaultAsync(a => a.HubSpotId == acccuntManagerHubSpotId);

		deal.FillOutAccountManager(accountManager);
	}

	private async Task<Result> GetDealFromHubSpotAsync(
		long supplierHubSpotId,
		long objectId,
		CancellationToken cancellationToken)
	{
		Result<string> accessTokenResult = await _hubSpotAuthorizationService
			.RefreshAccessTokenFromSupplierHubSpotIdAsync(
				supplierHubSpotId,
				_unitOfWork,
				cancellationToken);

		if (accessTokenResult.IsFailure)
		{
			return accessTokenResult;
		}

		Result<Deal> result = await _hubSpotDealService
			.GetByIdAsync(
				accessTokenResult.Value,
				objectId,
				cancellationToken);

		if (result.IsFailure)
		{
			return result;
		}

		Deal deal = await FillOutDealAssociations(result.Value, cancellationToken);

		await _unitOfWork
			.DealRepository
			.CreateAsync(deal, cancellationToken);

		await _unitOfWork.SaveAsync(cancellationToken);

		return Result.Success();
	}

	private async Task<Deal> FillOutDealAssociations(Deal deal, CancellationToken cancellationToken)
	{
		AccountManager? accountManager = await _unitOfWork
			.AccountManagerRepository
			.FirstOrDefaultAsync(a => a.HubSpotId == deal.HubSpotOwnerId, cancellationToken);

		List<Contact>? contacts = new();

		foreach (DealContact dc in deal.DealContacts)
		{
			Contact? contact = await _unitOfWork
				.ContactRepository
				.FirstOrDefaultAsync(c => c.HubSpotId == dc.HubSpotContactId);

			if (contact is not null)
			{
				contacts.Add(contact);
			}
		}

		Client? client = null;

		if (deal.Client is not null)
		{
			client = await _unitOfWork
				.ClientRepository
				.FirstOrDefaultAsync(c => c.HubSpotId == deal.Client.HubSpotId, cancellationToken);
		}

		deal.FillOutAssociations(accountManager, client, contacts);

		return deal;
	}
}
