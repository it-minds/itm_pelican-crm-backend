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

	public async Task<Result> Handle(UpdateDealCommand command, CancellationToken cancellationToken)
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
		Supplier? supplier = await _unitOfWork
				.SupplierRepository
				.FirstOrDefaultAsync(
					supplier => supplier.HubSpotId.ToString() == userId,
					cancellationToken);

		if (supplier is null
			|| string.IsNullOrWhiteSpace(supplier.RefreshToken))
		{
			return Result.Failure<Deal>(Error.NullValue);
		}

		Result<string> accessTokenResult = await _hubSpotAuthorizationService
			.RefreshAccessTokenAsync(
				supplier.RefreshToken,
				cancellationToken);

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

		await _unitOfWork
			.DealRepository
			.CreateAsync(result.Value, cancellationToken);

		await _unitOfWork.SaveAsync(cancellationToken);

		return Result.Success();
	}
}
