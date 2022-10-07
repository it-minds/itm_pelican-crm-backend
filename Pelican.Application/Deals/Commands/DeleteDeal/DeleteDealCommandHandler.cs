using Pelican.Application.Abstractions.Messaging;
using Pelican.Domain.Entities;
using Pelican.Domain.Repositories;
using Pelican.Domain.Shared;

namespace Pelican.Application.Deals.Commands.DeleteDeal;

internal sealed class DeleteDealCommandHandler : ICommandHandler<DeleteDealCommand>
{
	private readonly IRepositoryWrapper _repositoryWrapper;

	public DeleteDealCommandHandler(IRepositoryWrapper repositoryWrapper)
	{
		_repositoryWrapper = repositoryWrapper ?? throw new ArgumentNullException(nameof(repositoryWrapper));
	}

	public async Task<Result> Handle(
		DeleteDealCommand command,
		CancellationToken cancellationToken)
	{
		Deal? deal = _repositoryWrapper
			.Deal
			.FindByCondition(d => d.Id.ToString() == command.ObjectId.ToString())
			.FirstOrDefault();

		if (deal is null)
		{
			return Result.Success();
		}

		_repositoryWrapper
			.Deal
			.Delete(deal);

		// wait for async impl and await that.
		_repositoryWrapper.Save();

		return Result.Success();
	}
}
