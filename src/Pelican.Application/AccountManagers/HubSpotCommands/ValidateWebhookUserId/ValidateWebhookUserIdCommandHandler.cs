using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Abstractions.HubSpot;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Domain.Entities;
using Pelican.Domain.Shared;

namespace Pelican.Application.AccountManagers.Commands.ValidateWebhookUserId;

internal sealed class ValidateWebhookUserIdCommandHandler : ICommandHandler<ValidateWebhookUserIdCommand>
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IHubSpotAuthorizationService _hubSpotAuthorizationService;
	private readonly IHubSpotOwnersService _hubSpotAccountManagerService;

	public ValidateWebhookUserIdCommandHandler(
		IUnitOfWork unitOfWork,
		IHubSpotAuthorizationService hubSpotAuthorizationService,
		IHubSpotOwnersService hubSpotAccountManagerService)
	{
		_unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
		_hubSpotAuthorizationService = hubSpotAuthorizationService ?? throw new ArgumentNullException(nameof(hubSpotAuthorizationService));
		_hubSpotAccountManagerService = hubSpotAccountManagerService ?? throw new ArgumentNullException(nameof(hubSpotAccountManagerService));
	}

	public async Task<Result> Handle(ValidateWebhookUserIdCommand request, CancellationToken cancellationToken)
	{
		AccountManager? accountManager = await _unitOfWork
			.AccountManagerRepository
			.FirstOrDefaultAsync(a => a.SourceUserId == request.UserId);

		if (accountManager is not null)
		{
			return Result.Success();
		}

		Supplier? supplier = await _unitOfWork
			.SupplierRepository
			.FirstOrDefaultAsync(s => s.SourceId == request.SupplierHubSpotId, cancellationToken);

		if (supplier is null)
		{
			return Result.Failure(Error.NullValue);
		}

		Result<string> accessTokenResult = await _hubSpotAuthorizationService.RefreshAccessTokenFromRefreshTokenAsync(supplier.RefreshToken, cancellationToken);

		if (accessTokenResult.IsFailure)
		{
			return accessTokenResult;
		}

		Result<AccountManager> newAccountManagerResult = await _hubSpotAccountManagerService.GetByUserIdAsync(accessTokenResult.Value, request.UserId, cancellationToken);

		if (newAccountManagerResult.IsFailure)
		{
			return newAccountManagerResult;
		}

		AccountManager newAccountManager = newAccountManagerResult.Value;
		newAccountManager.Supplier = supplier;
		newAccountManager.SupplierId = supplier.Id;

		await _unitOfWork
			.AccountManagerRepository
			.CreateAsync(newAccountManagerResult.Value, cancellationToken);

		await _unitOfWork
			.SaveAsync(cancellationToken);

		return Result.Success();
	}
}
