using Pelican.Application.Abstractions.Messaging;
using Pelican.Domain.Entities;
using Pelican.Domain.Repositories;
using Pelican.Domain.Shared;

namespace Pelican.Application.Deals.Commands.DeleteDeal;

public sealed record DeleteDealCommand(
	long ObjectId) : ICommand;


internal sealed class DeleteDealCommandHandler : ICommandHandler<DeleteDealCommand>
{
	private readonly IDealRepository _dealRepository;

	public DeleteDealCommandHandler(IDealRepository dealRepository)
	{
		_dealRepository = dealRepository ?? throw new ArgumentNullException(nameof(dealRepository));
	}

	public async Task<Result> Handle(
		DeleteDealCommand command,
		CancellationToken cancellationToken)
	{
		Deal? deal = _dealRepository
			.FindByCondition(d => d.Id.ToString() == command.ObjectId.ToString())
			.FirstOrDefault();

		if (deal is null)
		{
			return Result.Success();
		}

		_dealRepository.Delete(deal);

		return Result.Success();
	}
}
