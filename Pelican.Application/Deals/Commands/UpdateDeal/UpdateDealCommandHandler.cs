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
		_unitOfWork = unitOfWork;
		_hubSpotDealService = hubSpotDealService;
		_hubSpotAuthorizationService = hubSpotAuthorizationService;
	}
	public async Task<Result> Handle(UpdateDealCommand command, CancellationToken cancellationToken)
	{
		Deal? deal = _unitOfWork
			.DealRepository
			.FindByCondition(d => d.Id.ToString() == command.ObjectId.ToString())
			.FirstOrDefault();

		if (deal is null)
		{
			return await GetDealFromHubSpot(
				command.PortalId,
				command.ObjectId,
				cancellationToken);
		}

		switch (command.PropertyName)
		{
			case "closedate":
				deal.EndDate = new DateTime(Convert.ToInt64(command.PropertyValue), DateTimeKind.Utc);
				break;
			case "amount":
				deal.Revenue = Convert.ToDecimal(command.PropertyValue);
				break;
			case "dealstage":
				deal.DealStatus = command.PropertyValue;
				break;
			case "deal_currency_code":
				deal.CurrencyCode = command.PropertyValue;
				break;
			default:
				break;
		}

		_unitOfWork
			.DealRepository
			.Update(deal);

		await _unitOfWork.SaveAsync(cancellationToken);

		return Result.Success();
	}

	private async Task<Result> GetDealFromHubSpot(long portalId, long objectId, CancellationToken cancellationToken)
	{
		Supplier? supplier = _unitOfWork
				.SupplierRepository
				.FindByCondition(supplier => supplier.HubSpotId == portalId)
				.FirstOrDefault();

		if (supplier is null || supplier.RefreshToken is null or "")
		{
			return Result.Failure<Deal>(Error.NullValue);
		}

		Result<string> accessTokenResult = await _hubSpotAuthorizationService
			.RefreshAccessTokenAsync(supplier.RefreshToken, cancellationToken);

		if (accessTokenResult.IsFailure)
		{
			return Result.Failure<Deal>(accessTokenResult.Error);
		}

		Result<Deal> result = await _hubSpotDealService.GetByIdAsync(accessTokenResult.Value, objectId, cancellationToken);

		if (result.IsFailure)
		{
			return Result.Failure<Deal>(result.Error);
		}

		await _unitOfWork
			.DealRepository
			.CreateAsync(result.Value, cancellationToken);

		return Result.Success();
	}
}
