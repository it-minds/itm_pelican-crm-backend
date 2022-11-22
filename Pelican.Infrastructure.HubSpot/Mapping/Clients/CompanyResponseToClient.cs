using Pelican.Domain;
using Pelican.Domain.Entities;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Clients;

namespace Pelican.Infrastructure.HubSpot.Mapping.Clients;

internal static class CompanyResponseToClient
{
	internal static Client ToClient(this CompanyResponse response)
	{
		if (string.IsNullOrWhiteSpace(response.Properties.HubSpotObjectId)
			|| string.IsNullOrWhiteSpace(response.Properties.Name))
		{
			throw new ArgumentNullException(nameof(response));
		}

		Client result = new(Guid.NewGuid())
		{
			Website = response.Properties.Domain.Length > StringLengths.Url ? response.Properties.Domain.Substring(0, StringLengths.Url - 3) + ("...") : response.Properties.Domain,
			Name = response.Properties.Name.Length > StringLengths.Name ? response.Properties.Name.Substring(0, StringLengths.Name - 3) + ("...") : response.Properties.Name,
			HubSpotId = response.Properties.HubSpotObjectId,
			OfficeLocation = response.Properties.City.Length > StringLengths.OfficeLocation ? response.Properties.City.Substring(0, StringLengths.OfficeLocation - 3) + ("...") : response.Properties.City,
		};

		result.Deals = response
			.Associations
			.Deals
			.AssociationList
			.Where(association => association.Type == "company_to_deal")
			.Select(association => new Deal(Guid.NewGuid())
			{
				HubSpotId = association.Id,
				Client = result,
				ClientId = result.Id,
			})
			.ToList();

		result.ClientContacts = response
			.Associations
			.Contacts
			.AssociationList
			.Where(association => association.Type == "company_to_contact")
			.Select(association => new ClientContact(Guid.NewGuid())
			{
				HubSpotContactId = association.Id,
				HubSpotClientId = result.HubSpotId,
				Client = result,
				ClientId = result.Id,
				IsActive = true,
			})
			.ToList();

		return result;
	}
}
