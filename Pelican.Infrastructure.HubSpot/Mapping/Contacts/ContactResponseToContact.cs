using Pelican.Domain.Entities;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Contacts;

namespace Pelican.Infrastructure.HubSpot.Mapping.Contacts;

internal static class ContactResponseToContact
{
	internal static Contact ToContact(this ContactResponse response)
	{
		if (string.IsNullOrWhiteSpace(response.Properties.HubSpotObjectId)
			|| string.IsNullOrWhiteSpace(response.Properties.HubSpotOwnerId))
		{
			throw new ArgumentNullException(nameof(response));
		}

		Contact result = new(Guid.NewGuid())
		{
			Firstname = response.Properties.Firstname,
			Lastname = response.Properties.Lastname,
			Email = response.Properties.Email,
			PhoneNumber = response.Properties.Phone,
			HubSpotId = response.Properties.HubSpotObjectId,
			JobTitle = response.Properties.JobTitle,
			HubSpotOwnerId = response.Properties.HubSpotOwnerId,
		};

		result.ClientContacts = response
			.Associations
			.Companies
			.AssociationList
			.Where(association => association.Type == "contact_to_company")
			.Select(association => new ClientContact(Guid.NewGuid())
			{
				ContactId = result.Id,
				HubSpotContactId = result.HubSpotId,
				Contact = result,
				IsActive = true,
				HubSpotClientId = association.Id,
			})
			.ToList();

		result.DealContacts = response
			.Associations
			.Deals
			.AssociationList
			.Where(association => association.Type == "contact_to_deal")
			.Select(association => new DealContact(Guid.NewGuid())
			{
				Contact = result,
				ContactId = result.Id,
				HubSpotContactId = result.HubSpotId,
				HubSpotDealId = association.Id,
				IsActive = true,
			})
			.ToList();

		return result;
	}
}
