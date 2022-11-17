using Pelican.Domain.Entities;
using Pelican.Infrastructure.HubSpot.Contracts.Responses.Deals;

namespace Pelican.Infrastructure.HubSpot.Mapping.Deals;

internal static class DealResponseToDeal
{
	internal static Deal ToDeal(this DealResponse response)
	{
		if (string.IsNullOrWhiteSpace(response.Properties.HubSpotObjectId))
		{
			throw new ArgumentNullException(nameof(response));
		}

		Deal result = new(Guid.NewGuid())
		{
			EndDate = string.IsNullOrWhiteSpace(response.Properties.CloseDate) ? null : DateTime.Parse(response.Properties.CloseDate),
			LastContactDate = string.IsNullOrWhiteSpace(response.Properties.LastContactDate) ? null : DateTime.Parse(response.Properties.LastContactDate),
			DealStatus = response.Properties.Dealstage,
			HubSpotId = response.Properties.HubSpotObjectId,
			HubSpotOwnerId = response.Properties.HubSpotOwnerId,
		};

		result.AccountManagerDeals = new List<AccountManagerDeal>()
		{
			new AccountManagerDeal(Guid.NewGuid())
			{
				Deal = result,
				DealId = result.Id,
				HubSpotDealId = result.HubSpotId,
				HubSpotAccountManagerId = result.HubSpotOwnerId,
				IsActive = true,
			}
		};

		result.DealContacts = response
			.Associations
			.Contacts
			.AssociationList
			.Where(association => association.Type == "deal_to_contact")
			.Select(association => new DealContact(Guid.NewGuid())
			{
				DealId = result.Id,
				HubSpotDealId = result.HubSpotId,
				Deal = result,
				HubSpotContactId = association.Id,
				IsActive = true,
			})
			.ToList();

		result.Client = response
			.Associations
			.Companies
			.AssociationList
			.Where(association => association.Type == "deal_to_company")
			.Select(association => new Client(Guid.NewGuid())
			{
				HubSpotId = association.Id,
				Deals = new List<Deal>() { result },
			})
			.FirstOrDefault();

		result.ClientId = result.Client?.Id;

		return result;
	}
}
