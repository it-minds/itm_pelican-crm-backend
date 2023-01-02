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
		_hubSpotAccountManagerService = hubSpotAccountManagerService
			?? throw new ArgumentNullException(nameof(hubSpotAccountManagerService));
		_hubSpotAuthorizationService = hubSpotAuthorizationService
			?? throw new ArgumentNullException(nameof(hubSpotAuthorizationService));
		_hubSpotContactService = hubSpotContactService
			?? throw new ArgumentNullException(nameof(hubSpotContactService));
		_hubSpotClientService = hubSpotClientService
			?? throw new ArgumentNullException(nameof(hubSpotClientService));
		_hubSpotDealService = hubSpotDealService
			 ?? throw new ArgumentNullException(nameof(hubSpotDealService));
		_unitOfWork = unitOfWork
			 ?? throw new ArgumentNullException(nameof(unitOfWork));
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

		Result<Supplier> supplierResult = await GetAndCreateSupplierAsync(tokensResult.Value.AccessToken, tokensResult.Value.RefreshToken, cancellationToken);

		if (supplierResult.IsFailure)
		{
			return supplierResult;
		}

		Result result = await GetAndCreateAccountManagersAsync(tokensResult.Value.AccessToken, supplierResult.Value, cancellationToken);

		if (result.IsFailure)
		{
			return result;
		}

		Result<List<Deal>> dealsResult = await GetAndCreateDealsAsync(tokensResult.Value.AccessToken, cancellationToken);

		if (dealsResult.IsFailure)
		{
			return dealsResult;
		}

		result = await GetAndCreateContactsAsync(tokensResult.Value.AccessToken, cancellationToken);

		if (result.IsFailure)
		{
			return result;
		}

		result = await GetAndCreateClientsAsync(tokensResult.Value.AccessToken, dealsResult.Value, cancellationToken);

		return result;
	}

	private async Task<Result<Supplier>> GetAndCreateSupplierAsync(
		string accessToken,
		string refreshToken,
		CancellationToken cancellationToken)
	{
		Result<Supplier> supplierResult = await _hubSpotAuthorizationService
			.DecodeAccessTokenAsync(accessToken, cancellationToken);

		if (supplierResult.IsFailure)
		{
			return supplierResult;
		}

		if (_unitOfWork
			.SupplierRepository
			.FindAll()
			.Any(supplier => supplier.SourceId == supplierResult.Value.SourceId && supplier.Source == Sources.HubSpot))
		{
			return Result.Failure<Supplier>(Error.AlreadyExists);
		}

		supplierResult.Value.RefreshToken = refreshToken;

		await _unitOfWork
			.SupplierRepository
			.CreateAsync(supplierResult.Value, cancellationToken);

		return supplierResult;
	}

	private async Task<Result> GetAndCreateAccountManagersAsync(
		string accessToken,
		Supplier supplier,
		CancellationToken cancellationToken)
	{

		Result<List<AccountManager>> accountManagersResult = await _hubSpotAccountManagerService
			.GetAsync(accessToken, cancellationToken);

		if (accountManagersResult.IsFailure)
		{
			return accountManagersResult;
		}

		accountManagersResult
			.Value
			.ForEach(a => a.Supplier = supplier);

		await _unitOfWork
		   .AccountManagerRepository
		   .CreateRangeAsync(accountManagersResult.Value, cancellationToken);

		await _unitOfWork.SaveAsync(cancellationToken);

		return Result.Success();
	}

	private async Task<Result<List<Deal>>> GetAndCreateDealsAsync(
		string accessToken,
		CancellationToken cancellationToken)
	{
		Result<List<Deal>> dealsResult = await _hubSpotDealService
			.GetAsync(accessToken, cancellationToken);

		if (dealsResult.IsFailure)
		{
			return dealsResult;
		}

		dealsResult
			.Value
			.SelectMany(d => d.AccountManagerDeals)
			.ToList()
			.ForEach(ad => ad.AccountManager = null!);

		await _unitOfWork
			.DealRepository
			.CreateRangeAsync(dealsResult.Value, cancellationToken);

		await _unitOfWork.SaveAsync(cancellationToken);

		return dealsResult;
	}

	private async Task<Result> GetAndCreateContactsAsync(
		string accessToken,
		CancellationToken cancellationToken)
	{
		Result<List<Contact>> contactsResult = await _hubSpotContactService
			.GetAsync(accessToken, cancellationToken);

		if (contactsResult.IsFailure)
		{
			return contactsResult;
		}

		contactsResult
			.Value
			.SelectMany(c => c.DealContacts)
			.ToList()
			.ForEach(dc => dc.Deal = null!);

		await _unitOfWork
			.ContactRepository
			.CreateRangeAsync(contactsResult.Value, cancellationToken);

		await _unitOfWork.SaveAsync(cancellationToken);

		return Result.Success();
	}

	private async Task<Result> GetAndCreateClientsAsync(
		string accessToken,
		List<Deal> deals,
		CancellationToken cancellationToken)
	{
		Result<List<Client>> clientsResult = await _hubSpotClientService
			.GetAsync(accessToken, cancellationToken);

		if (clientsResult.IsFailure)
		{
			return clientsResult;
		}

		clientsResult
			.Value
			.ForEach(c => c.Deals = deals
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
