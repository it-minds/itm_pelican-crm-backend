using Pelican.Application.Abstractions.HubSpot;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.Common.Interfaces.Repositories;
using Pelican.Domain.Entities;
using Pelican.Domain.Shared;

namespace Pelican.Application.Deals.Commands.UpdateDeal;

internal sealed class UpdateDealCommandHandler : ICommandHandler<UpdateDealCommand>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IHubSpotDealService _hubSpotDealService;
	public UpdateDealCommandHandler(
		IUnitOfWork unitOfWork,
		IHubSpotDealService hubSpotDealService)
	{
		_unitOfWork = unitOfWork;
		_hubSpotDealService = hubSpotDealService;
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
				command.UserId,
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

	private async Task<Result> GetDealFromHubSpot(string userId, long objectId, CancellationToken cancellationToken)
	{
		Supplier? supplier = _unitOfWork
				.SupplierRepository
				.FindByCondition(supplier => supplier.HubSpotId.ToString() == userId)
				.FirstOrDefault();

		if (supplier is null)
		{
			return Result.Failure<Deal>(Error.NullValue);
		}

		string token = supplier.RefreshToken;

		Result<Deal> result = await _hubSpotDealService.GetDealByIdAsync(token, objectId, cancellationToken);

		if (result.IsFailure)
		{
			return Result.Failure<Deal>(result.Error);
		}

		_unitOfWork
			.DealRepository
			.Create(result.Value);

		return Result.Success();
	}
}
