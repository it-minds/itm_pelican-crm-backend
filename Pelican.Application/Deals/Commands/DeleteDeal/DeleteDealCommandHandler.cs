using Pelican.Application.Abstractions.Messaging;
using Pelican.Domain.Entities;
using Pelican.Domain.Repositories;
using Pelican.Domain.Shared;

namespace Pelican.Application.Deals.Commands.DeleteDeal;

internal sealed class DeleteDealCommandHandler : ICommandHandler<DeleteDealCommand>
{
	private readonly IDealRepository _dealRepository;

	public DeleteDealCommandHandler(IDealRepository dealRepository)
	{
		_dealRepository = dealRepository ?? throw new ArgumentNullException(nameof(dealRepository));
	}

	public Task<Result> Handle(
		DeleteDealCommand command,
		CancellationToken cancellationToken)
	{
		Deal? deal = _dealRepository
			.FindByCondition(d => d.Id.ToString() == command.ObjectId.ToString())
			.FirstOrDefault();

		if (deal is null)
		{
			return new Task<Result>(() => Result.Success());
		}

		_dealRepository.Delete(deal);

		return new Task<Result>(() => Result.Success());
	}
}
