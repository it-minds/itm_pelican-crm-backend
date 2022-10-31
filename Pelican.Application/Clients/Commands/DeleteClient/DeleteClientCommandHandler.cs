using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.Common.Interfaces.Repositories;
using Pelican.Domain.Entities;
using Pelican.Domain.Shared;

namespace Pelican.Application.Clients.Commands.DeleteClient;
internal sealed class DeleteClientCommandHandler : ICommandHandler<DeleteClientCommand>
{
	private readonly IUnitOfWork _unitOfWork;

	public DeleteClientCommandHandler(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(IUnitOfWork));
	}

	public async Task<Result> Handle(
		DeleteClientCommand command,
		CancellationToken cancellationToken)
	{
		Client? client = _unitOfWork
			.ClientRepository
			.FindByCondition(d => d.Id.ToString() == command.ObjectId.ToString())
			.FirstOrDefault();

		if (client is null)
		{
			return Result.Success();
		}

		_unitOfWork
			.ClientRepository
			.Delete(client);

		await _unitOfWork.SaveAsync(cancellationToken);

		return Result.Success();
	}
}
