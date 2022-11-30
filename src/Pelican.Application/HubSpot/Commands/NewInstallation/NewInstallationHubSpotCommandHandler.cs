using System.Runtime.CompilerServices;
using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Abstractions.HubSpot;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.HubSpot.Dtos;
using Pelican.Domain.Entities;
using Pelican.Domain.Shared;

[assembly: InternalsVisibleTo("Pelican.Application.Test")]
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

		if (supplierResult.IsFailure)
		{
			return supplierResult;
		}

		if (_unitOfWork.SupplierRepository.FindAll().Any(supplier => supplier.HubSpotId == supplierResult.Value.HubSpotId))
		{
			return Result.Failure(Error.AlreadyExists);
		}

		Result<List<AccountManager>> accountManagersResult = await _hubSpotAccountManagerService
			.GetAsync(accessToken, cancellationToken);

		Result<List<Contact>> contactsResult = await _hubSpotContactService
			.GetAsync(accessToken, cancellationToken);

		Result<List<Client>> clientsResult = await _hubSpotClientService
			.GetAsync(accessToken, cancellationToken);

		Result<List<Deal>> dealsResult = await _hubSpotDealService
			.GetAsync(accessToken, cancellationToken);


		if (Result.FirstFailureOrSuccess(new Result[]
				{
					accountManagersResult,
					contactsResult,
					clientsResult,
					dealsResult,
				}) is Result result
			&& result.IsFailure)
		{
			return result;
		}

		Supplier supplier = supplierResult.Value;
		List<AccountManager> accountManagers = accountManagersResult.Value;
		List<Contact> contacts = contactsResult.Value;
		List<Client> clients = clientsResult.Value;
		List<Deal> deals = dealsResult.Value;

		supplier.RefreshToken = tokensResult.Value.RefreshToken;

		accountManagers
			.ForEach(accountManager =>
			{
				accountManager.Supplier = supplier;
				accountManager.SupplierId = supplier.Id;

				accountManager.AccountManagerDeals = deals
					.Where(deal => deal.HubSpotOwnerId == accountManager.HubSpotId)?
					.Select(deal => new AccountManagerDeal(Guid.NewGuid())
					{
						AccountManager = accountManager,
						AccountManagerId = accountManager.Id,
						HubSpotAccountManagerId = accountManager.HubSpotId,
						Deal = deal,
						DealId = deal.Id,
						HubSpotDealId = deal.HubSpotId,
						IsActive = true,
					})
					.ToList() ?? new List<AccountManagerDeal>();
			});

		supplier.AccountManagers = accountManagers;

		List<AccountManagerDeal> accountManagerDeals = accountManagers
			.SelectMany(accountManager => accountManager.AccountManagerDeals)
			.ToList();

		deals
			.ForEach(deal =>
			{
				deal.AccountManagerDeals = accountManagerDeals
					.Where(accountManagerDeal => accountManagerDeal.HubSpotDealId == deal.HubSpotId)?
					.ToList() ?? new List<AccountManagerDeal>();

				if (deal.Client is not null)
				{
					deal.Client = clients
						.FirstOrDefault(client => client.HubSpotId == deal.Client.HubSpotId);
				}

				deal
					.DealContacts?
					.ToList()
					.ForEach(dealContact =>
					{
						Contact contact = contacts
							.First(contact => contact.HubSpotId == dealContact.HubSpotContactId);

						dealContact.Contact = contact;
						dealContact.ContactId = contact.Id;
					});
			});

		clients
			.ForEach(client =>
			{
				client.Deals = deals
					.Where(deal => deal.Client?.HubSpotId == client.HubSpotId)?
					.ToList() ?? new List<Deal>();

				client
					.ClientContacts?
					.ToList()
					.ForEach(clientContact =>
					{
						Contact contact = contacts
							.First(contact => contact.HubSpotId == clientContact.HubSpotContactId);

						clientContact.Contact = contact;
						clientContact.ContactId = contact.Id;
					});
			});

		List<DealContact> dealContacts = deals
			.SelectMany(deal => deal.DealContacts)
			.ToList();

		List<ClientContact> clientContacts = clients
			.SelectMany(client => client.ClientContacts)
			.ToList();

		contacts
			.ForEach(contact =>
			{
				contact.DealContacts = dealContacts
					.Where(dealContact => dealContact.HubSpotContactId == contact.HubSpotId)
					.ToList();

				contact.ClientContacts = clientContacts
					.Where(clientContact => clientContact.HubSpotContactId == contact.HubSpotId)
					.ToList();
			});

		await _unitOfWork.SupplierRepository.CreateAsync(supplier, cancellationToken);
		await _unitOfWork.AccountManagerRepository.CreateRangeAsync(accountManagers, cancellationToken);
		await _unitOfWork.DealRepository.CreateRangeAsync(deals, cancellationToken);
		await _unitOfWork.ClientRepository.CreateRangeAsync(clients, cancellationToken);
		await _unitOfWork.ContactRepository.CreateRangeAsync(contacts, cancellationToken);

		await _unitOfWork.SaveAsync(cancellationToken);

		return Result.Success();
	}
}
