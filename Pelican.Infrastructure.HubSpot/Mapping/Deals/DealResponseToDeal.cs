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
			StartDate = string.IsNullOrWhiteSpace(response.Properties.StartDate) ? null : Convert.ToDateTime(response.Properties.StartDate).Ticks,
			EndDate = string.IsNullOrWhiteSpace(response.Properties.EndDate) ? null : Convert.ToDateTime(response.Properties.EndDate).Ticks,
			LastContactDate = string.IsNullOrWhiteSpace(response.Properties.LastContactDate) ? null : Convert.ToDateTime(response.Properties.LastContactDate).Ticks,
			HubSpotId = response.Properties.HubSpotObjectId,
			HubSpotOwnerId = response.Properties.HubSpotOwnerId,
		};

		result.Name = response.Properties.DealName;
		result.Description = response.Properties.Description;
		result.DealStatus = response.Properties.DealStage;

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
