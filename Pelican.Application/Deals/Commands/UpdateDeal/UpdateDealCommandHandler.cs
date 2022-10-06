using Pelican.Application.Abstractions.HubSpot;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Domain.Entities;
using Pelican.Domain.Repositories;
using Pelican.Domain.Shared;

namespace Pelican.Application.Deals.Commands.UpdateDeal;

internal sealed class UpdateDealCommandHandler : ICommandHandler<UpdateDealCommand>
{
	private readonly IAccountManagerRepository _accountManagerRepository;
	private readonly IDealRepository _dealRepository;
	private readonly IHubSpotDealService _hubSpotDealService;
	public UpdateDealCommandHandler(
		IAccountManagerRepository accountManagerRepository,
		IDealRepository dealRepository,
		IHubSpotDealService hubSpotDealService)
	{
		_accountManagerRepository = accountManagerRepository;
		_dealRepository = dealRepository;
		_hubSpotDealService = hubSpotDealService;
	}
	public async Task<Result> Handle(UpdateDealCommand command, CancellationToken cancellationToken)
	{
		Deal? deal = _dealRepository
			.FindByCondition(d => d.Id.ToString() == command.ObjectId.ToString())
			.FirstOrDefault();

		if (deal is null)
		{
			if (command.UserId is null) return Result.Failure(Error.NullValue);

			AccountManager accountManager = _accountManagerRepository
				.FindByCondition(a => a.Id.ToString() == command.UserId.ToString())
				.First();

			/// account manager should hold refreshtoken
			string token = "";

			Result<Deal> result
				= await _hubSpotDealService.GetDealByIdAsync(token, command.ObjectId, cancellationToken);

			if (result.IsFailure) return Result.Failure(result.Error);

			_dealRepository.Create(result.Value);
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

		_dealRepository.Update(deal);

		return Result.Success();
	}
}
