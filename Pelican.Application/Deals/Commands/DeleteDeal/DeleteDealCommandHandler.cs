using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.Common.Interfaces.Repositories;
using Pelican.Domain.Entities;
using Pelican.Domain.Shared;

namespace Pelican.Application.Deals.Commands.DeleteDeal;

internal sealed class DeleteDealCommandHandler : ICommandHandler<DeleteDealCommand>
{
	private readonly IUnitOfWork _unitOfWork;

	public DeleteDealCommandHandler(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
	}

	public async Task<Result> Handle(
		DeleteDealCommand command,
		CancellationToken cancellationToken)
	{
		Deal? deal = _unitOfWork
			.DealRepository
			.FindByCondition(d => d.Id.ToString() == command.ObjectId.ToString())
			.FirstOrDefault();

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
