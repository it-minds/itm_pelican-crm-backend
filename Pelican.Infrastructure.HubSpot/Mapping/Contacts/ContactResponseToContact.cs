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
			FirstName = response.Properties.FirstName.Length > StringLengths.Name ? response.Properties.FirstName.Substring(0, StringLengths.Name - 3) + ("...") : response.Properties.FirstName,
			LastName = response.Properties.LastName.Length > StringLengths.Name ? response.Properties.LastName.Substring(0, StringLengths.Name - 3) + ("...") : response.Properties.LastName,
			Email = response.Properties.Email.Length > StringLengths.Email ? response.Properties.Email.Substring(0, StringLengths.Email - 3) + ("...") : response.Properties.Email,
			PhoneNumber = response.Properties.Phone.Length > StringLengths.PhoneNumber ? response.Properties.Phone.Substring(0, StringLengths.PhoneNumber - 3) + ("...") : response.Properties.Phone,
			HubSpotId = response.Properties.HubSpotObjectId,
			JobTitle = response.Properties.JobTitle.Length > StringLengths.JobTitle ? response.Properties.JobTitle.Substring(0, StringLengths.JobTitle - 3) + ("...") : response.Properties.JobTitle,
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
