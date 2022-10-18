using System.Runtime.CompilerServices;
using Pelican.Application.Abstractions.HubSpot;
using Pelican.Application.Abstractions.Messaging;
using Pelican.Application.Common.Interfaces.Repositories;
using Pelican.Application.HubSpot.Dtos;
using Pelican.Domain.Entities;
using Pelican.Domain.Shared;

[assembly: InternalsVisibleTo("Pelican.Application.Test")]
namespace Pelican.Application.HubSpot.Commands.NewInstallation;

internal sealed class NewInstallationCommandHandler : ICommandHandler<NewInstallationCommand>
{
	private readonly IHubSpotObjectService<AccountManager> _hubSpotAccountManagerService;
	private readonly IHubSpotAuthorizationService _hubSpotAuthorizationService;
	private readonly IHubSpotObjectService<Contact> _hubSpotContactService;
	private readonly IHubSpotObjectService<Client> _hubSpotClientService;
	private readonly IHubSpotObjectService<Deal> _hubSpotDealService;
	private readonly IUnitOfWork _unitOfWork;

	public NewInstallationCommandHandler(
		IHubSpotObjectService<AccountManager> hubSpotAccountManagerService,
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
		NewInstallationCommand command,
		CancellationToken cancellationToken)
	{
		Result<RefreshAccessTokens> tokensResult = await _hubSpotAuthorizationService
			.AuthorizeUserAsync(command.Code, cancellationToken);

		if (tokensResult.IsFailure)
		{
			return Result.Failure(tokensResult.Error);
		}

		/// missing check to see if supplier is already loaded

		string accessToken = tokensResult.Value.AccessToken;

		Result<Supplier> supplierResult = await _hubSpotAuthorizationService
			.DecodeAccessTokenAsync(accessToken, cancellationToken);

		Result<IEnumerable<AccountManager>> accountManagersResult = await _hubSpotAccountManagerService
			.GetAsync(accessToken, cancellationToken);

		Result<IEnumerable<Contact>> contactsResult = await _hubSpotContactService
			.GetAsync(accessToken, cancellationToken);

		Result<IEnumerable<Client>> clientsResult = await _hubSpotClientService
			.GetAsync(accessToken, cancellationToken);

		Result<IEnumerable<Deal>> dealsResult = await _hubSpotDealService
			.GetAsync(accessToken, cancellationToken);


		if (Result.FirstFailureOrSuccess(new Result[]
				{
					supplierResult,
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
		List<AccountManager> accountManagers = accountManagersResult.Value.ToList();
		List<Deal> deals = dealsResult.Value.ToList();
		List<Contact> contacts = contactsResult.Value.ToList();
		List<Client> clients = clientsResult.Value.ToList();

		supplier.RefreshToken = tokensResult.Value.RefreshToken;

		foreach (AccountManager accountManager in accountManagers)
		{
			accountManager.Supplier = supplier;
			accountManager.SupplierId = supplier.Id;

			accountManager.AccountManagerDeals = deals
				.Where(deal => deal.HubSpotOwnerId == accountManager.HubSpotId)
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
		}

		supplier.AccountManagers = accountManagers;

		foreach (Deal deal in deals)
		{
			deal.AccountManagerDeals = accountManagers
				.SelectMany(accountManager => accountManager.AccountManagerDeals)
				.Where(accountManagerDeal => accountManagerDeal.DealId == deal.Id)
				.ToList() ?? new List<AccountManagerDeal>();

			deal.Client = clients
				.First(client => client.HubSpotId == deal.Client.HubSpotId);

			foreach (DealContact dealContact in deal.DealContacts)
			{
				Contact contact = contacts
					.First(contact => contact.HubSpotId == dealContact.HubSpotContactId);

				dealContact.Contact = contact;
				dealContact.ContactId = contact.Id;
			}
		}

		foreach (Contact contact in contacts)
		{
			contact.DealContacts = deals
			.SelectMany(deal => deal.DealContacts)
			.Where(dealContact => dealContact.HubSpotContactId == contact.HubSpotId)
			.ToList();

			foreach (ClientContact clientContact in contact.ClientContacts)
			{
				Client client = clients
					.First(client => client.HubSpotId == clientContact.HubspotClientId);

				clientContact.Client = client;
				clientContact.ClientId = client.Id;
			}
		}

		foreach (Client client in clients)
		{
			client.ClientContacts = contacts
				.SelectMany(contact => contact.ClientContacts)
				.Where(clientContact => clientContact.ClientId == client.Id)
				.ToList();
		}

		await _unitOfWork.SupplierRepository.CreateAsync(supplier, cancellationToken);

		await _unitOfWork.SaveAsync(cancellationToken);

		return Result.Success();
	}
}
