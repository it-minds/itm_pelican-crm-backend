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
				HubSpotContactId = result.HubSpotId,
				Contact = result,
				IsActive = true,
				HubSpotClientId = company.Id,
			})
			.ToList() ?? new List<ClientContact>();

		result.DealContacts = response
			.Associations?
			.Deals?
			.AssociationList
			.Where(deal => deal.Type == "contact_to_deal")?
			.Select(deal => new DealContact(Guid.NewGuid())
			{
				Contact = result,
				ContactId = result.Id,
				HubSpotContactId = result.HubSpotId,
				HubSpotDealId = deal.Id,
				IsActive = true,
			})
			.ToList() ?? new List<DealContact>();

		return result;
	}
}
