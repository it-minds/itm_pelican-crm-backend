using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.Common.Interfaces.Repositories;
using Pelican.Domain.Entities;
using Pelican.Domain.Shared;

namespace Pelican.Application.Suppliers.Commands.DeleteSupplier;

internal sealed class DeleteSupplierCommandHandler : ICommandHandler<DeleteSupplierCommand>
{
	private readonly IUnitOfWork _unitOfWork;

	public DeleteSupplierCommandHandler(IUnitOfWork unitOfWork)
	{
		_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(IUnitOfWork));
	}

	public async Task<Result> Handle(
		DeleteSupplierCommand command,
		CancellationToken cancellationToken)
	{
		Supplier? supplier = _unitOfWork
			.SupplierRepository
			.FindByCondition(d => d.Id.ToString() == command.ObjectId.ToString())
			.FirstOrDefault();

		if (supplier is null)
		{
			return Result.Success();
		}

		_unitOfWork
			.SupplierRepository
			.Delete(supplier);

		await _unitOfWork.SaveAsync(cancellationToken);

		return Result.Success();
	}
}
