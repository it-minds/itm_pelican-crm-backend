using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.Common.Interfaces.Repositories;
using Pelican.Domain.Entities;
using Pelican.Domain.Shared;

namespace Pelican.Application.Contacts.Commands.DeleteContact;

internal sealed class DeleteContactCommandHandler : ICommandHandler<DeleteContactCommand>
{
	private readonly IUnitOfWork _unitOfWork;

	public DeleteContactCommandHandler(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(IUnitOfWork));
	}

	public async Task<Result> Handle(
		DeleteContactCommand command,
		CancellationToken cancellationToken)
	{
		Contact? contact = _unitOfWork
			.ContactRepository
			.FindByCondition(d => d.Id.ToString() == command.ObjectId.ToString())
			.FirstOrDefault();

		if (contact is null)
		{
			return Result.Success();
		}

		_unitOfWork
			.ContactRepository
			.Delete(contact);

		await _unitOfWork.SaveAsync(cancellationToken);

		return Result.Success();
	}
}
