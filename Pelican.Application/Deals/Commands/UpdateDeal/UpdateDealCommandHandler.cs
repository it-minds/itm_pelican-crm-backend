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
			AccountManager? accountManager = _unitOfWork
				.AccountManagerRepository
				.FindByCondition(a => a.Id.ToString() == command.UserId.ToString())
				.FirstOrDefault();

			if (accountManager is null) return Result.Failure(Error.NullValue);

			/// account manager should hold refreshtoken
			string token = "";

			Result<Deal> result
				= await _hubSpotDealService.GetDealByIdAsync(token, command.ObjectId, cancellationToken);

			if (result.IsFailure) return Result.Failure(result.Error);

			_unitOfWork
				.DealRepository
				.Create(result.Value);

			return Result.Success();
		}

		switch (command.PropertyName)
		{
			case "closedate":
				deal.EndDate = new DateTime(Convert.ToInt64(command.PropertyValue), DateTimeKind.Utc);
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
}
