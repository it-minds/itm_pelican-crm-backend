using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Domain.Entities;
using Pelican.Domain.Shared;

namespace Pelican.Application.Deals.Commands.DeleteDeal;

internal sealed class DeleteDealHubSpotCommandHandler : ICommandHandler<DeleteDealHubSpotCommand>
{
	private readonly IUnitOfWork _unitOfWork;

	public DeleteDealHubSpotCommandHandler(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
	}

	public async Task<Result> Handle(
		DeleteDealHubSpotCommand command,
		CancellationToken cancellationToken)
	{
		Deal? deal = await _unitOfWork
			.DealRepository
			.FirstOrDefaultAsync(d => d.HubSpotId == command.ObjectId.ToString(), cancellationToken);

		if (deal is null)
		{
			return Result.Success();
		}

		_unitOfWork
			.DealRepository
			.Delete(deal);

		await _unitOfWork.SaveAsync(cancellationToken);

		return Result.Success();
	}
}
