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
			SourceId = response.Properties.HubSpotObjectId,
			Name = response.Properties.Name,
			Website = response.Properties.Domain,
			OfficeLocation = response.Properties.City,
			Source = Sources.HubSpot,
		};

		result.Deals = response
			.Associations
			.Deals
			.AssociationList
			.Where(association => association.Type == "company_to_deal")
			.Select(association => new Deal(Guid.NewGuid())
			{
				SourceId = association.Id,
				Client = result,
				ClientId = result.Id,
				Source = Sources.HubSpot,
			})
			.ToList();

		result.ClientContacts = response
			.Associations
			.Contacts
			.AssociationList
			.Where(association => association.Type == "company_to_contact")
			.Select(association => new ClientContact(Guid.NewGuid())
			{
				SourceContactId = association.Id,
				SourceClientId = result.SourceId,
				Client = result,
				ClientId = result.Id,
				IsActive = true,
			})
			.ToList();

		return result;
	}
}
