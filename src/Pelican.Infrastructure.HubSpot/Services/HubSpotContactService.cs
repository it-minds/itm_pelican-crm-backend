using Pelican.Application.Abstractions.Data.Repositories;
using Pelican.Application.Abstractions.HubSpot;
using Pelican.Application.Abstractions.Infrastructure;
using Pelican.Domain.Entities;
using Pelican.Domain.Settings.HubSpot;
using Pelican.Domain.Shared;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Contacts;
using Pelican.Infrastructure.HubSpot.Extensions;
using Pelican.Infrastructure.HubSpot.Mapping.Contacts;
using RestSharp;

namespace Pelican.Infrastructure.HubSpot.Services;

internal sealed class HubSpotContactService : ServiceBase<HubSpotSettings>, IHubSpotObjectService<Contact>
{
	private readonly IUnitOfWork _unitOfWork;

	public HubSpotContactService(
		IClient<HubSpotSettings> hubSpotClient, IUnitOfWork unitOfWork)
		: base(hubSpotClient)
		=> _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));


	public async Task<Result<Contact>> GetByIdAsync(
		string accessToken,
		long id,
		CancellationToken cancellationToken)
	{
		RestRequest request = new RestRequest($"crm/v4/objects/contacts/{id}")
			.AddHeader("Authorization", $"Bearer {accessToken}")
			.AddContactQueryParams();

		IResponse<ContactResponse> response = await _client
			.GetAsync<ContactResponse>(request, cancellationToken);

		return await response
			.GetResultWithUnitOfWork(
				ContactResponseToContact.ToContact,
				_unitOfWork,
				cancellationToken);
	}

	public async Task<Result<List<Contact>>> GetAsync(
		string accessToken,
		CancellationToken cancellationToken)
	{
		RestRequest request = new RestRequest("crm/v4/objects/contacts")
			.AddHeader("Authorization", $"Bearer {accessToken}")
			.AddContactQueryParams();

		IResponse<ContactsResponse> response = await _client
			.GetAsync<ContactsResponse>(request, cancellationToken);

		return await response
			.GetResultWithUnitOfWork(
				ContactsResponseToContacts.ToContacts,
				_unitOfWork,
				cancellationToken);
	}
}
