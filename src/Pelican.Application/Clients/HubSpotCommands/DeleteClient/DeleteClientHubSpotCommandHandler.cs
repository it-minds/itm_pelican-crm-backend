using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Domain.Entities;
using Pelican.Domain.Shared;

namespace Pelican.Application.Clients.HubSpotCommands.DeleteClient;
internal sealed class DeleteClientHubSpotCommandHandler : ICommandHandler<DeleteClientHubSpotCommand>
{
	private readonly IUnitOfWork _unitOfWork;

	public DeleteClientHubSpotCommandHandler(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
	}

	public async Task<Result> Handle(
		DeleteClientHubSpotCommand command,
		CancellationToken cancellationToken)
	{
		Client? client = _unitOfWork
			.ClientRepository
			.FindByCondition(d => d.SourceId == command.ObjectId.ToString())
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
