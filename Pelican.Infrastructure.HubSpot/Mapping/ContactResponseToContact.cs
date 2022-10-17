using Pelican.Domain.Entities;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Contacts;

namespace Pelican.Infrastructure.HubSpot.Mapping;

internal static class ContactResponseToContact
{
	internal static Contact ToContact(this ContactResponse response)
	{
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
			.Associations?
			.Companies?
			.AssociationList
			.Where(company => company.Type == "contact_to_company")?
			.Select(company => new ClientContact(Guid.NewGuid())
			{
				ContactId = result.Id,
				HubspotContactId = result.HubSpotId,
				Contact = result,
				IsActive = true,
				HubspotClientId = company.Id,
			})
			.ToList() ?? new List<ClientContact>();

		return result;
	}
}
