using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Abstractions.HubSpot;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.HubSpot.Dtos;
using Pelican.Domain;
using Pelican.Domain.Entities;
using Pelican.Domain.Shared;

namespace Pelican.Application.HubSpot.Commands.NewInstallation;

internal sealed class NewInstallationHubSpotCommandHandler : ICommandHandler<NewInstallationHubSpotCommand>
{
	private readonly IHubSpotOwnersService _hubSpotAccountManagerService;
	private readonly IHubSpotAuthorizationService _hubSpotAuthorizationService;
	private readonly IHubSpotObjectService<Contact> _hubSpotContactService;
	private readonly IHubSpotObjectService<Client> _hubSpotClientService;
	private readonly IHubSpotObjectService<Deal> _hubSpotDealService;
	private readonly IUnitOfWork _unitOfWork;

	public NewInstallationHubSpotCommandHandler(
		IHubSpotOwnersService hubSpotAccountManagerService,
		IHubSpotAuthorizationService hubSpotAuthorizationService,
		IHubSpotObjectService<Contact> hubSpotContactService,
		IHubSpotObjectService<Client> hubSpotClientService,
		IHubSpotObjectService<Deal> hubSpotDealService,
		IUnitOfWork unitOfWork)
	{
		_hubSpotAccountManagerService = hubSpotAccountManagerService;
		_hubSpotAuthorizationService = hubSpotAuthorizationService;
		_hubSpotContactService = hubSpotContactService;
		_hubSpotClientService = hubSpotClientService;
		_hubSpotDealService = hubSpotDealService;
		_unitOfWork = unitOfWork;
	}

	public async Task<Result> Handle(
		NewInstallationHubSpotCommand command,
		CancellationToken cancellationToken)
	{
		Result<RefreshAccessTokens> tokensResult = await _hubSpotAuthorizationService
			.AuthorizeUserAsync(command.Code, cancellationToken);

		if (tokensResult.IsFailure)
		{
			return tokensResult;
		}

		string accessToken = tokensResult.Value.AccessToken;

		Result<Supplier> supplierResult = await _hubSpotAuthorizationService
			.DecodeAccessTokenAsync(accessToken, cancellationToken);

		if (supplierResult.IsFailure) return supplierResult;

		if (_unitOfWork
			.SupplierRepository
			.FindAll()
			.Any(supplier => supplier.SourceId == supplierResult.Value.SourceId && supplier.Source == Sources.HubSpot))
		{
			return Result.Failure(Error.AlreadyExists);
		}

		supplierResult.Value.RefreshToken = tokensResult.Value.RefreshToken;

		await _unitOfWork
			.SupplierRepository
			.CreateAsync(supplierResult.Value, cancellationToken);

		Result<List<AccountManager>> accountManagersResult = await _hubSpotAccountManagerService
			.GetAsync(accessToken, cancellationToken);

		if (accountManagersResult.IsFailure) return accountManagersResult;

		supplierResult.Value.AccountManagers = accountManagersResult.Value;

		accountManagersResult
			.Value
			.ForEach(a => a.Supplier = supplierResult.Value);

		await _unitOfWork
		   .AccountManagerRepository
		   .CreateRangeAsync(accountManagersResult.Value, cancellationToken);

		await _unitOfWork.SaveAsync(cancellationToken);

		Result<List<Deal>> dealsResult = await _hubSpotDealService
			.GetAsync(accessToken, cancellationToken);

		if (dealsResult.IsFailure) return dealsResult;

		dealsResult
			.Value
			.SelectMany(d => d.AccountManagerDeals)
			.ToList()
			.ForEach(ad => ad.AccountManager = null!);

		await _unitOfWork
			.DealRepository
			.CreateRangeAsync(dealsResult.Value, cancellationToken);

		await _unitOfWork.SaveAsync(cancellationToken);

		Result<List<Contact>> contactsResult = await _hubSpotContactService
			.GetAsync(accessToken, cancellationToken);

		if (contactsResult.IsFailure) return contactsResult;

		contactsResult
			.Value
			.SelectMany(c => c.DealContacts)
			.ToList()
			.ForEach(dc => dc.Deal = null!);

		await _unitOfWork
			.ContactRepository
			.CreateRangeAsync(contactsResult.Value, cancellationToken);

		await _unitOfWork.SaveAsync(cancellationToken);

		Result<List<Client>> clientsResult = await _hubSpotClientService
			.GetAsync(accessToken, cancellationToken);

		if (clientsResult.IsFailure) return clientsResult;

		clientsResult
			.Value
			.ForEach(c => c.Deals = dealsResult
				.Value
				.Where(d => c.Deals.Any(dd => dd.SourceId == d.SourceId))
				.ToList());

		clientsResult
			.Value
			.SelectMany(c => c.ClientContacts)
			.ToList()
			.ForEach(cc => cc.Contact = null!);

		await _unitOfWork
			.ClientRepository
			.CreateRangeAsync(clientsResult.Value, cancellationToken);

		await _unitOfWork.SaveAsync(cancellationToken);

		return Result.Success();
	}
}
