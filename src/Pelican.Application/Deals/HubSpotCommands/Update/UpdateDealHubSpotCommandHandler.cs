namespace Pelican.Application.Deals.HubSpotCommands.Update;

internal sealed class UpdateDealHubSpotCommandHandler : ICommandHandler<UpdateDealHubSpotCommand>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IHubSpotObjectService<Deal> _hubSpotDealService;
	private readonly IHubSpotAuthorizationService _hubSpotAuthorizationService;

	public UpdateDealHubSpotCommandHandler(
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

	public async Task<Result> Handle(
		UpdateDealHubSpotCommand command,
		CancellationToken cancellationToken = default)
	{
		Deal? deal = _unitOfWork
			.DealRepository
			.FindByCondition(d => d.SourceId == command.ObjectId.ToString() && d.Source == Sources.HubSpot)
			.Include(d => d.AccountManagerDeals)
			.Include(d => d.Client)
			.Include(d => d.DealContacts)
			.FirstOrDefault();

		if (deal is null)
		{
			return await GetAndCreateDealAsync(
				command.SupplierHubSpotId,
				command.ObjectId,
				cancellationToken);
		}

		if ((deal.LastUpdatedAt ?? deal.CreatedAt) <= command.UpdateTime)
		{
			deal.UpdateProperty(
					command.PropertyName,
					command.PropertyValue);

			if (command.PropertyName == "hs_all_owner_ids")
			{
				await UpdateAccountManagerDealAsync(
					deal,
					command.PropertyValue,
					cancellationToken);
			}
		}
		else
		{
			Result<Deal> result = await GetDealFromHubSpotAsync(
				command.SupplierHubSpotId,
				command.ObjectId,
				cancellationToken);

			if (result.IsFailure)
			{
				return result;
			}

			deal.UpdatePropertiesFromDeal(result.Value);
			deal.UpdateAccountManager(result.Value.AccountManagerDeals.FirstOrDefault()?.AccountManager);
		}

		_unitOfWork
			.DealRepository
			.Attach(deal);

		await _unitOfWork.SaveAsync(cancellationToken);

		return Result.Success();
	}

	private async Task UpdateAccountManagerDealAsync(
		Deal deal,
		string accountManagerHubSpotId,
		CancellationToken cancellationToken)
	{
		if (deal.ActiveAccountManagerDeal?.SourceAccountManagerId == accountManagerHubSpotId)
		{
			return;
		}

		AccountManager? accountManager = await _unitOfWork
			.AccountManagerRepository
			.FirstOrDefaultAsync(
				a => a.SourceId == accountManagerHubSpotId && a.Source == Sources.HubSpot,
				cancellationToken);

		deal.UpdateAccountManager(accountManager);
	}

	private async Task<Result> GetAndCreateDealAsync(
		long supplierHubSpotId,
		long objectId,
		CancellationToken cancellationToken)
	{
		Result<Deal> result = await GetDealFromHubSpotAsync(
			supplierHubSpotId,
			objectId,
			cancellationToken);

		if (result.IsFailure)
		{
			return result;
		}

		_unitOfWork
			.DealRepository
			.Attach(result.Value);

		await _unitOfWork.SaveAsync(cancellationToken);

		return Result.Success();
	}

	private async Task<Result<Deal>> GetDealFromHubSpotAsync(
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
			return Result.Failure<Deal>(accessTokenResult.Error);
		}

		return await _hubSpotDealService
			.GetByIdAsync(
				accessTokenResult.Value,
				objectId,
				cancellationToken);
	}
}
