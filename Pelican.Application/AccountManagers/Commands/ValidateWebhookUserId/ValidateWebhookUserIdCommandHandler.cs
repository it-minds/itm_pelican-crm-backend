using Pelican.Application.Abstractions.HubSpot;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.Common.Interfaces.Repositories;
using Pelican.Domain.Entities;
using Pelican.Domain.Shared;

namespace Pelican.Application.AccountManagers.Commands.ValidateWebhookUserId;

internal sealed class ValidateWebhookUserIdCommandHandler : ICommandHandler<ValidateWebhookUserIdCommand>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHubSpotAuthorizationService _hubSpotAuthorizationService;
    private readonly IHubSpotObjectService<AccountManager> _hubSpotAccountManagerService;

    public ValidateWebhookUserIdCommandHandler(
        IUnitOfWork unitOfWork,
        IHubSpotAuthorizationService hubSpotAuthorizationService,
        IHubSpotObjectService<AccountManager> hubSpotAccountManagerService)
    {
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork)); ;
        _hubSpotAuthorizationService = hubSpotAuthorizationService ?? throw new ArgumentNullException(nameof(hubSpotAuthorizationService));
        _hubSpotAccountManagerService = hubSpotAccountManagerService ?? throw new ArgumentNullException(nameof(hubSpotAccountManagerService)); ;
    }

    public async Task<Result> Handle(ValidateWebhookUserIdCommand request, CancellationToken cancellationToken)
    {
        AccountManager? accountManager = await _unitOfWork
            .AccountManagerRepository
            .FirstOrDefaultAsync(a => a.HubSpotUserId == request.UserId);

        if (accountManager is not null)
        {
            return Result.Success();
        }

        Result<string> accessToken = "";// = _hubSpotAuthorizationService.RefreshAccessTokenAsync(request.SupplierHubSpotId, _unitOfWork, cancellationToken);

        if (accessToken.IsFailure)
        {
            return accessToken;
        }

        Result<AccountManager> newAccountManager = await _hubSpotAccountManagerService.GetByIdAsync(accessToken.Value, request.UserId, cancellationToken);

        if (newAccountManager.IsFailure)
        {
            return newAccountManager;
        }

        await _unitOfWork
            .AccountManagerRepository
            .CreateAsync(newAccountManager.Value, cancellationToken);

        await _unitOfWork
            .SaveAsync(cancellationToken);

        return Result.Success();
    }
}
