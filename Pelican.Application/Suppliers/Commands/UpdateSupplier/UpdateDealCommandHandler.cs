using Pelican.Application.Abstractions.HubSpot;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.Common.Interfaces.Repositories;
using Pelican.Domain.Entities;
using Pelican.Domain.Shared;

namespace Pelican.Application.Suppliers.Commands.UpdateSupplier;

internal sealed class UpdateSupplierCommandHandler : ICommandHandler<UpdateSupplierCommand>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IHubSpotSupplierService _hubSpotSupplierService;
	public UpdateSupplierCommandHandler(
		IUnitOfWork unitOfWork,
		IHubSpotSupplierService hubSpotSupplierService)
	{
		_unitOfWork = unitOfWork;
		_hubSpotSupplierService = hubSpotSupplierService;
	}
	public async Task<Result> Handle(UpdateSupplierCommand command, CancellationToken cancellationToken)
	{
		Supplier? supplier = _unitOfWork
			.SupplierRepository
			.FindByCondition(d => d.Id.ToString() == command.ObjectId.ToString())
			.FirstOrDefault();

		if (supplier is null)
		{
			return await GetSupplierFromHubSpot(
				command.UserId,
				command.ObjectId,
				cancellationToken);
		}

		switch (command.PropertyName)
		{

			default:
				break;
		}

		_unitOfWork
			.SupplierRepository
			.Update(supplier);

		await _unitOfWork.SaveAsync(cancellationToken);

		return Result.Success();
	}

	private async Task<Result> GetSupplierFromHubSpot(string userId, long objectId, CancellationToken cancellationToken)
	{
		Supplier? supplier = _unitOfWork
				.SupplierRepository
				.FindByCondition(supplier => supplier.HubSpotId.ToString() == userId)
				.FirstOrDefault();

		if (supplier is null)
		{
			return Result.Failure<Supplier>(Error.NullValue);
		}

		string token = supplier.RefreshToken;

		Result<Supplier> result = await _hubSpotSupplierService.GetSupplierByIdAsync(token, objectId, cancellationToken);

		if (result.IsFailure)
		{
			return Result.Failure<Supplier>(result.Error);
		}

		_unitOfWork
			.SupplierRepository
			.Create(result.Value);

		return Result.Success();
	}
}
