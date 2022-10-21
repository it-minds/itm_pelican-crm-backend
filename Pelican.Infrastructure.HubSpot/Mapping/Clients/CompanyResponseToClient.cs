using Pelican.Domain.Entities;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Clients;

namespace Pelican.Infrastructure.HubSpot.Mapping.Clients;

internal static class CompanyResponseToClient
{
	internal static Client ToClient(this CompanyResponse response)
	{
		Client result = new(Guid.NewGuid())
		{
			Name = response.Properties.Name,
			HubSpotId = response.Properties.HubSpotObjectId,
			OfficeLocation = response.Properties.City,
			Segment = response.Properties.Industry,
		};

		result.Deals = response
			.Associations?
			.Deals?
			.AssociationList?
			.Where(deal => deal.Type == "company_to_deal")?
			.Select(deal => new Deal(Guid.NewGuid())
			{
				HubSpotId = deal.Id,
				Client = result,
				ClientId = result.Id,
			})
			.ToList()
			?? new List<Deal>();

		result.ClientContacts = response
			.Associations?
			.Contacts?
			.AssociationList?
			.Where(contact => contact.Type == "company_to_contact")?
			.Select(contact => new ClientContact(Guid.NewGuid())
			{
				HubSpotContactId = contact.Id,
				HubSpotClientId = result.HubSpotId,
				Client = result,
				ClientId = result.Id,
				IsActive = true,
			})
			.ToList()
			?? new List<ClientContact>();

		return result;
	}
}
