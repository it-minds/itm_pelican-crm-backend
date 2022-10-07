using Pelican.Application.Abstractions.HubSpot;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Domain.Entities;
using Pelican.Domain.Repositories;
using Pelican.Domain.Shared;

namespace Pelican.Application.Deals.Commands.UpdateDeal;

internal sealed class UpdateDealCommandHandler : ICommandHandler<UpdateDealCommand>
{
	private readonly IRepositoryWrapper _repositoryWrapper;
	private readonly IHubSpotDealService _hubSpotDealService;
	public UpdateDealCommandHandler(
IRepositoryWrapper repositoryWrapper,
		IHubSpotDealService hubSpotDealService)
	{
		_repositoryWrapper = repositoryWrapper;
		_hubSpotDealService = hubSpotDealService;
	}
	public async Task<Result> Handle(UpdateDealCommand command, CancellationToken cancellationToken)
	{
		Deal? deal = _repositoryWrapper
			.Deal
			.FindByCondition(d => d.Id.ToString() == command.ObjectId.ToString())
			.FirstOrDefault();

		if (deal is null)
		{
			AccountManager? accountManager = _repositoryWrapper
				.AccountManager
				.FindByCondition(a => a.Id.ToString() == command.UserId.ToString())
				.FirstOrDefault();

			if (accountManager is null) return Result.Failure(Error.NullValue);

			/// account manager should hold refreshtoken
			string token = "";

			Result<Deal> result
				= await _hubSpotDealService.GetDealByIdAsync(token, command.ObjectId, cancellationToken);

			if (result.IsFailure) return Result.Failure(result.Error);


			_repositoryWrapper
				.Deal
				.Create(result.Value);

			return Result.Success();
		}

		switch (command.PropertyName)
		{
			case "dealname":
				break;
			case "closedate":
				deal.EndDate = new DateTime(Convert.ToInt64(command.PropertyValue), DateTimeKind.Utc);
				break;
			default:
				break;
		}

		_repositoryWrapper.Deal.Update(deal);

		_repositoryWrapper.Save();

		return Result.Success();
	}
}
