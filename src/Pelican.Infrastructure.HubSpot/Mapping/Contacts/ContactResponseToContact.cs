using Pelican.Domain;
using Pelican.Domain.Entities;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Contacts;

namespace Pelican.Infrastructure.HubSpot.Mapping.Contacts;

internal static class ContactResponseToContact
{
	internal static Contact ToContact(this ContactResponse response)
	{
		if (string.IsNullOrWhiteSpace(response.Properties.HubSpotObjectId))
		{
			throw new ArgumentNullException(nameof(response));
		}

		Contact result = new(Guid.NewGuid())
		{
			FirstName = response.Properties.FirstName,
			LastName = response.Properties.LastName,
			Email = response.Properties.Email,
			PhoneNumber = response.Properties.Phone,
			SourceId = response.Properties.HubSpotObjectId,
			JobTitle = response.Properties.JobTitle,
			SourceOwnerId = response.Properties.HubSpotOwnerId,
			Source = Sources.HubSpot,
		};

		result.ClientContacts = response
			.Associations
			.Companies
			.AssociationList
			.Where(association => association.Type == "contact_to_company")
			.Select(association => new ClientContact(Guid.NewGuid())
			{
				ContactId = result.Id,
				SourceContactId = result.SourceId,
				Contact = result,
				IsActive = true,
				SourceClientId = association.Id,
				Source = Sources.HubSpot,
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
				SourceContactId = result.SourceId,
				SourceDealId = association.Id,
				IsActive = true,
				Source = Sources.HubSpot,
			})
			.ToList();

		return result;
	}
}
