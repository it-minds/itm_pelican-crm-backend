using Pelican.Application.Abstractions.HubSpot;
using Pelican.Domain.Entities;
using Pelican.Domain.Shared;
using Pelican.Infrastructure.HubSpot.Abstractions;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Contacts;
using Pelican.Infrastructure.HubSpot.Extensions;
using Pelican.Infrastructure.HubSpot.Mapping.Contacts;
using RestSharp;

namespace Pelican.Infrastructure.HubSpot.Services;

internal sealed class HubSpotContactService : HubSpotService, IHubSpotObjectService<Contact>
{
	public HubSpotContactService(
		IHubSpotClient hubSpotClient) : base(hubSpotClient)
	{ }

	public async Task<Result<Contact>> GetByIdAsync(
		string accessToken,
		long id,
		CancellationToken cancellationToken)
	{
		RestRequest request = new RestRequest($"crm/v4/objects/contacts/{id}")
			.AddHeader("Authorization", $"Bearer {accessToken}")
			.AddContactQueryParams();

		RestResponse<ContactResponse> response = await _hubSpotClient
			.GetAsync<ContactResponse>(request, cancellationToken);

		return response
			.GetResult(ContactResponseToContact.ToContact);
	}

	public async Task<Result<List<Contact>>> GetAsync(
		string accessToken,
		CancellationToken cancellationToken)
	{
		RestRequest request = new RestRequest("crm/v4/objects/contacts")
			.AddHeader("Authorization", $"Bearer {accessToken}")
			.AddContactQueryParams();

		RestResponse<ContactsResponse> response = await _hubSpotClient
			.GetAsync<ContactsResponse>(request, cancellationToken);

		return response
			.GetResult(ContactsResponseToContacts.ToContacts);
	}
}
